using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;
using System.Globalization;

namespace AsymCryptLab1 {

//----------ClassProgram----------------------------------------------------------------------------------------------------------------------------------------|

    class Program {

//----------------------------------------------------------------------------------------------------|
		
        static void Main(string[] args) {

            Generator.CSharpPseudoRandom();
            Generator.LemerLow();
            Generator.LemerHigh();
            Generator.L20();
            Generator.L89();
            Generator.Geffe();
            Generator.Librarian();
            Generator.Wolfram();
            Generator.BM();
            Generator.BM_Bytes();
            Generator.BBS();
            Generator.BBS_Bytes();

            Console.ReadLine();
		}
	}
//---------ClassGenerator---------------------------------------------------------------------------------------------------------------------------------------|

    class Generator {

        static int million = 1_000_000;
        static double[] alpha = { 0.01, 0.05, 0.1 };
        static double[] quantile = { 2.326, 1.645, 1.282 };

//----------------------------------------------------------------------------------------------------| 1)

        public static void CSharpPseudoRandom () {

            Console.WriteLine("--C# pseudo-random generator is working...-------------------------------------------\n");
            
            byte[] bytes = new byte[million];
            
            Random rnd = new Random();

            rnd.NextBytes(bytes);

            //for (int i = 0; i < 1_000_000; i++) Console.Write(bytes[i] + " ");                        //show all random bytes
 
            Analysis(bytes);
        }
//----------------------------------------------------------------------------------------------------| 2.1)

        public static void LemerLow () {
        
            Console.WriteLine("LemerLow generator is working...---------------------------------------------\n");

            ulong m = (1L << 32) - 1;
            ulong a = (1 << 16) + 1;
            ulong c = 119;

            ulong x = 0b11001100010100011011011000000101;

            byte[] bytes = new byte[million];

            for (int i = 0; i<million; i++) {
            
                x = (a*x + c)&m;

                bytes[i] = Convert.ToByte(x&0b11111111);
            }

            Analysis(bytes);
        }
//----------------------------------------------------------------------------------------------------| 2.2)

        public static void LemerHigh () {
        
            Console.WriteLine("LemerHigh generator is working...---------------------------------------------\n");

            ulong m = (1L << 32) - 1;
            ulong a = (1 << 16) + 1;
            ulong c = 119;

            ulong x = 0b11001100010100011011011000000101;

            byte[] bytes = new byte[million];

            for (int i = 0; i<million; i++) {
            
                x = (a*x + c)&m;

                bytes[i] = Convert.ToByte(x >> 24);
            }

            Analysis(bytes);
        }
//----------------------------------------------------------------------------------------------------| 3)

		public static void L20 () {

            Console.WriteLine("--L20 generator is working...----------------------------------------------------\n");

            bool[] sequence = new bool[8*million];

            sequence[1] = true;

            for (int i = 20; i < sequence.Length; i++) {

                sequence[i] = sequence[i - 3] ^ sequence[i - 5] ^ sequence[i - 9] ^ sequence[i - 20];
            }

            byte[] bytes = new byte[million];

            int j = 0;

            for (int i = 0; i < bytes.Length; i++) {

                for (int k = 0; k < 8; k++) {

                    bytes[i] |= Convert.ToByte(Convert.ToByte(sequence[j + k]) << k);
                }

                j += 8;
            }

            Analysis(bytes);
        }
//----------------------------------------------------------------------------------------------------| 4)

        public static void L89 () {

            Console.WriteLine("--L89 generator is working...----------------------------------------------------\n");

            bool[] sequence = new bool[8*million];

            for (int i = 45; i < 70; i++) sequence[i] = true;

            for (int i = 89; i < sequence.Length; i++) {
            
                sequence[i] = sequence[i-38] ^ sequence[i-89];
            }

            byte[] bytes = new byte[million];

            int j = 0;

            for (int i = 0; i < bytes.Length; i++) {

                for (int k = 0; k < 8; k++) {

                    bytes[i] |= Convert.ToByte(Convert.ToByte(sequence[j + k]) << k);
                }

                j += 8;
            }

            Analysis(bytes);
        }
//----------------------------------------------------------------------------------------------------| 5)
    
        public static void Geffe () {
        
            Console.WriteLine("Geffe generator is working...------------------------------------------------------------\n");

            bool[] L9 = new bool[8 * million];

            for (int i = 2; i < 6; i++) L9[i] = true;

            for (int i = 9; i < L9.Length; i++) {
            
                L9[i] = L9[i-9] ^ L9[i-8] ^ L9[i-6] ^ L9[i-5];
            }

            bool[] L10 = new bool[8 * million];

            for (int i = 7; i < 10; i++) L10[i] = true;

            for (int i = 10; i < L10.Length; i++) {

                L10[i] = L10[i - 10] ^ L10[i - 7];
            }

            bool[] L11 = new bool[8 * million];

            for (int i = 10; i < 11; i++) L11[i] = true;

            for (int i = 11; i < L11.Length; i++) {

                L11[i] = L11[i - 11] ^ L11[i - 9];
            }

            bool[] Geffe = new bool[8*million];

            for (int i = 0; i<Geffe.Length; i++) {
            
                Geffe[i] = (L10[i]&L11[i]) ^ ((true^L10[i])&L9[i]);
            }

            byte[] bytes = new byte[million];

            int j = 0;

            for (int i = 0; i < bytes.Length; i++) {

                for (int k = 0; k < 8; k++) {

                    bytes[i] |= Convert.ToByte(Convert.ToByte(Geffe[j + k]) << k);
                }

                j += 8;
            }

            Analysis(bytes);
        }
//----------------------------------------------------------------------------------------------------| 6)

        public static void Wolfram () {

            Console.WriteLine("Wolfram generator is working...------------------------------------------------------------\n");

            uint r = 666;

            bool[] sequence = new bool[8*million];

            for (int i = 0; i<sequence.Length; i++) {
            
                sequence[i] = Convert.ToBoolean(1&r);

                r = ((r << 1) | (r >> 31)) ^ (r | ((r >> 1) | (1&r)<<31));
            }

            byte[] bytes = new byte[million];

            int j = 0;

            for (int i = 0; i < bytes.Length; i++) {

                for (int k = 0; k < 8; k++) {

                    bytes[i] |= Convert.ToByte(Convert.ToByte(sequence[j + k]) << k);
                }

                j += 8;
            }

            Analysis(bytes);
        }
//----------------------------------------------------------------------------------------------------| 7)

        public static void Librarian () {

            Console.WriteLine("Librarian generator is working...----------------------------------------\n");

            StreamReader cp = new StreamReader(@"C:\Users\PRIDE\source\repos\AsymCryptLab1\Melville Herman. Moby Dick.txt");

            string curLine = "";
            string book = "";
            while ((curLine = cp.ReadLine()) != null) book += curLine;
            cp.Close();

            byte[] bytes = new byte[million];

            for (int i = 0; i< million; i++) {
            
                bytes[i] = Convert.ToByte(book[i]);
            }

            Analysis(bytes);
        }
//----------------------------------------------------------------------------------------------------| 8.1)

        public static void BM () {

            Console.WriteLine("BM generator is working...--------------------------------------------------------\n");

            BigInteger p = BigInteger.Parse("00CEA42B987C44FA642D80AD9F51F10457690DEF10C83D0BC1BCEE12FC3B6093E3", NumberStyles.HexNumber);
            BigInteger a = BigInteger.Parse("5B88C41246790891C095E2878880342E88C79974303BD0400B090FE38A688356", NumberStyles.HexNumber);

            BigInteger T = RandomBigInteger(p-1);

            bool[] sequence = new bool[million];

            BigInteger chk = (p-1)/2;

            for (int i = 0; i < million; i++) {
            
                if (T.CompareTo(chk) < 0) sequence[i] = true;
                T = BigInteger.ModPow(a, T, p);
            }

            byte[] bytes = new byte[million/8];

            int j = 0;

            for (int i = 0; i < bytes.Length; i++) {

                for (int k = 0; k < 8; k++) {

                    bytes[i] |= Convert.ToByte(Convert.ToByte(sequence[j + k]) << k);
                }

                j += 8;
            }

            Analysis(bytes);
        }
//----------------------------------------------------------------------------------------------------| 8.2)

        public static void BM_Bytes() {

            Console.WriteLine("BM_Bytes generator is working...--------------------------------------------------------\n");

            BigInteger p = BigInteger.Parse("00CEA42B987C44FA642D80AD9F51F10457690DEF10C83D0BC1BCEE12FC3B6093E3", NumberStyles.HexNumber);
            BigInteger a = BigInteger.Parse("5B88C41246790891C095E2878880342E88C79974303BD0400B090FE38A688356", NumberStyles.HexNumber);

            BigInteger T = RandomBigInteger(p - 1);

            BigInteger chk = (p - 1) / 256;

            byte[] bytes = new byte[million / 8];

            for (int i = 0; i < million/8; i++) {

                for (int k = 0; k < 256; k++) {
                
                    if ((T.CompareTo(k*chk) >= 0) && (T.CompareTo((k+1)*chk) < 0)) {
                        
                        bytes[i] = (byte) k; 
                        break;
                    }
                }

                T = BigInteger.ModPow(a, T, p);
            }

            Analysis(bytes);
        }
//----------------------------------------------------------------------------------------------------| 9.1)

        public static void BBS() {
            
            Console.WriteLine("BBS generator is working....................................................................\n");

            BigInteger p = BigInteger.Parse("00D5BBB96D30086EC484EBA3D7F9CAEB07", NumberStyles.HexNumber);
            BigInteger q = BigInteger.Parse("425D2B9BFDB25B9CF6C416CC6E37B59C1F", NumberStyles.HexNumber);
            BigInteger n = p*q;

            BigInteger r = n - p - q;

            bool[] sequence = new bool[8*million];

            for (int i = 0; i < sequence.Length; i++) {
            
                r = BigInteger.ModPow(r, 2, n);
                sequence[i] = Convert.ToBoolean((byte)(1&r));
            }

            byte[] bytes = new byte[million];

            int j = 0;

            for (int i = 0; i < bytes.Length; i++) {

                for (int k = 0; k < 8; k++) {

                    bytes[i] |= Convert.ToByte(Convert.ToByte(sequence[j + k]) << k);
                }

                j += 8;
            }

            Analysis(bytes);
        }
//----------------------------------------------------------------------------------------------------| 9.2)

        public static void BBS_Bytes() {
            
            Console.WriteLine("BBS_Bytes generator is working....................................................................\n");

            BigInteger p = BigInteger.Parse("00D5BBB96D30086EC484EBA3D7F9CAEB07", NumberStyles.HexNumber);
            BigInteger q = BigInteger.Parse("425D2B9BFDB25B9CF6C416CC6E37B59C1F", NumberStyles.HexNumber);
            BigInteger n = p*q;

            BigInteger r = n - p - q;

            byte[] bytes = new byte[million];

            for (int i = 0; i < bytes.Length; i++) {
            
                r = BigInteger.ModPow(r, 2, n);
                bytes[i] = (byte)((0b11111111)&r);
            }

            Analysis(bytes);
        }
//----------------------------------------------------------------------------------------------------|

        static void Analysis (byte[] bytes) { 
        
            UniformAnalysis(bytes);
            IndependenceAnalysis(bytes);
            HomogeneityAnalysis(bytes);            
        }
//----------------------------------------------------------------------------------------------------|

        static void UniformAnalysis(byte[] bytes) {

            Console.WriteLine("I. Uniform analysis is requested...-----------------------------------------------------------\n");

            Dictionary<byte, int> Frequency = new Dictionary<byte, int>();

            for (int i = 0; i < 256; i++) Frequency.Add(Convert.ToByte(i), 0);

            for (int i = 0; i < bytes.Length; i++) Frequency[bytes[i]]++;

            double chi_square = 0;
            double n = bytes.Length / 256;

            for (int i = 0; i < 256; i++) chi_square += Math.Pow(Frequency[Convert.ToByte(i)] - n, 2);

            chi_square /= n;

            Console.WriteLine("Chi-Square is: {0}\n", chi_square);

            double chi_square_alpha = 0;
            int l = 255;

            for (int i = 0; i < 3; i++) {

                chi_square_alpha = Math.Sqrt(2 * l) * quantile[i] + l;

                Console.WriteLine("{0}) When alpha = {1}, Chi-Square_alpha = {2}\n", i, alpha[i], chi_square_alpha);
                if (chi_square <= chi_square_alpha) Console.WriteLine("Since Chi-Square is less or equal than Chi-Square_alpha, hypothesis that random sequence is uniform should not be descarded.\n");
                else Console.WriteLine("Since Chi-Square is more than Chi-Square_alpha, hypothesis that random sequence is uniform should be descarded.\n");
            }
        }
//----------------------------------------------------------------------------------------------------|

        static void IndependenceAnalysis(byte[] bytes) {

            Console.WriteLine("II. Indepence analysis is requested...-----------------------------------------------------------\n");

            var Frequency = new Dictionary<(byte, byte), int>();

            for (int i = 0; i < 256; i++) for (int j = 0; j < 256; j++) Frequency.Add((Convert.ToByte(i), Convert.ToByte(j)), 0);

            int n = bytes.Length / 2;

            for (int i = 1; i < 2*n; i+=2) Frequency[(bytes[i-1], bytes[i])]++;

            double chi_square = 0;

            for (int i = 0; i < 256; i++) {

                int vi = 0;

                for (int j = 0; j < 256; j++) vi+= Frequency[(Convert.ToByte(i), Convert.ToByte(j))];

                for (int j = 0; j < 256; j++) {

                    int vj = 0;

                    for (int k = 0; k < 256; k++) vj += Frequency[(Convert.ToByte(k), Convert.ToByte(j))];

                    double temp = Math.Pow(Frequency[(Convert.ToByte(i), Convert.ToByte(j))], 2) / (vi * vj);
                    
                    if (double.IsNaN(temp) || temp < 0) continue;

                    chi_square += temp;

                }
            }

            chi_square = n * (chi_square - 1);

            Console.WriteLine("Chi-Square is: {0}\n", chi_square);

            double chi_square_alpha = 0;
            int l = 255*255;

            for (int i = 0; i < 3; i++) {

                chi_square_alpha = Math.Sqrt(2 * l) * quantile[i] + l;

                Console.WriteLine("{0}) When alpha = {1}, Chi-Square_alpha = {2}\n", i, alpha[i], chi_square_alpha);
                if (chi_square <= chi_square_alpha) Console.WriteLine("Since Chi-Square is less or equal than Chi-Square_alpha, hypothesis that random sequence is independent should not be descarded.\n");
                else Console.WriteLine("Since Chi-Square is more than Chi-Square_alpha, hypothesis that random sequence is independent should be descarded.\n");
            }
        }
//----------------------------------------------------------------------------------------------------|

        static void HomogeneityAnalysis(byte[] bytes) {

            int r = 10;

            Console.WriteLine("III. Homogeneity analysis for r = {0} is requested...-----------------------------------------------------------\n", r);

            int m = bytes.Length / r;
            int n = m*r;

            Dictionary<byte, int>[] Frequency = new Dictionary<byte, int>[r];

            for (int k = 0; k < r; k++) {

                Frequency[k] = new Dictionary<byte, int>();

                for (int i = 0; i < 256; i++) Frequency[k].Add(Convert.ToByte(i), 0);
            }
            
            for (int k = 0; k < r; k++) for (int i = k*m; i < (k+1)*m; i++) Frequency[k][bytes[i]]++;

            double chi_square = 0;

            for (int i = 0; i < 256; i++) {

                int vi = 0;

                for (int j = 0; j < r; j++) vi += Frequency[j][Convert.ToByte(i)];

                for (int j = 0; j < r; j++) {

                    int vij = Frequency[j][Convert.ToByte(i)];

                    double temp = Math.Pow(vij, 2) / (vi * m);

                    if (double.IsNaN(temp) || temp < 0) continue;

                    chi_square += temp;

                }
            }

            chi_square = n * (chi_square - 1);

            Console.WriteLine("Chi-Square is: {0}\n", chi_square);

            double chi_square_alpha = 0;
            int l = 255 * (r-1);

            for (int i = 0; i < 3; i++) {

                chi_square_alpha = Math.Sqrt(2 * l) * quantile[i] + l;

                Console.WriteLine("{0}) When alpha = {1}, Chi-Square_alpha = {2}\n", i, alpha[i], chi_square_alpha);
                if (chi_square <= chi_square_alpha) Console.WriteLine("Since Chi-Square is less or equal than Chi-Square_alpha, hypothesis that random sequence is homogeneous for r = {0} for should not be descarded.\n", r);
                else Console.WriteLine("Since Chi-Square is more than Chi-Square_alpha, hypothesis that random sequence is homogeneous for r = {0} should be descarded.\n", r);
            }
        }
//----------------------------------------------------------------------------------------------------| function's taken from https://cswiki.cs.byu.edu/cs-312/randombigintegers

        static BigInteger RandomBigInteger(BigInteger N) {

            Random rand = new Random();

            BigInteger result = 0;
            do {
                int length = (int)Math.Ceiling(BigInteger.Log(N, 2));
                int numBytes = (int)Math.Ceiling(length / 8.0);
                byte[] data = new byte[numBytes];
                rand.NextBytes(data);
                result = new BigInteger(data);
            } while (result >= N || result <= 0);
            return result;
        }
    }
//----------------------------------------------------------------------------------------------------|
}
//--------------------------------------------------------------------------------------------------------------------------------------------------------------|
