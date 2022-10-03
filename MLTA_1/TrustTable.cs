using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLTA_1
{
    public class TrustTable
    {
        public int varCount { get; private set; }
        public string[] varArray { get; private set; }
        public bool[,] trustTable { get; private set; }
        public int trustTableRowCount { get; private set; }
        public bool[] result { get; private set; }
        public bool[] dualityResult { get; private set; }
        public TrustTable(int count)
        {
            varArray = new string[count];
            varCount = count;
            trustTableRowCount = (int)Math.Pow(2, varCount);
            result = new bool[trustTableRowCount];
            trustTable = new bool[trustTableRowCount, varCount];
            for (int i = 0; i < varCount; i++)
            {
                varArray[i] = Convert.ToChar(65 + i).ToString();
                trustTable[0, i] = false;
            }
            FillTable();
        }
        public TrustTable(string function)
        {
            HashSet<string> vars = new HashSet<string>();
            for (int i = 0; i < function.Length; i++)
            {
                if (char.IsLetter(function[i]))
                {
                    vars.Add(function[i].ToString());
                }
            }
            varArray = vars.ToArray();
            varCount = varArray.Length;
            trustTableRowCount = (int)Math.Pow(2, varCount);
            result = new bool[trustTableRowCount];
            trustTable = new bool[trustTableRowCount, varCount];
            for (int i = 0; i < varCount; i++)
            {
                trustTable[0, i] = false;
            }
            FillTable();
            for (int i = 0; i < trustTableRowCount; i++)
            {
                string func = "";
                Dictionary<string, string> varsValues = new Dictionary<string, string>();
                for (int j = 0; j < varCount; j++)
                {
                    string value = "0";
                    if (trustTable[i, j])
                    {
                        value = "1";
                    }
                    varsValues.Add(varArray[j], value);                 
                }
                for (int k = 0; k < function.Length; k++)
                {
                    if (varsValues.ContainsKey(function[k].ToString()))
                        func += varsValues[function[k].ToString()];
                    else
                        func += function[k];
                }
                Expression ex = new Expression(func);
                result[i] = ex.Solve();
            }

        }
        public void Duality()
        {
            dualityResult = result.Reverse().ToArray();
            for (int i = 0; i < dualityResult.Length; i++)
            {
                dualityResult[i] = !dualityResult[i];
            }
            
        }
        public bool IsSelfDuality()
        {
            bool[] up = new bool[result.Length / 2];
            bool[] down = new bool[result.Length / 2];
            for (int i = 0; i < result.Length / 2; i++)
            {
                up[i] = result[i];
            }
            for (int i = 0; i < result.Length / 2; i++)
            {
                down[i] = result[result.Length / 2 + i];
            }
            for (int i = 0; i < result.Length /2; i++)
            {
                if(up[i] == down[result.Length / 2  - 1 - i])
                {
                    return false;
                }
            }
            return true;
        }
        private void FillTable()
        {
            
            for (int j = 1; j < trustTableRowCount; j++)
            {
                bool tmp = true;
                for (int i = varCount-1; i >= 0; i--)
                {
                    trustTable[j, i] = (trustTable[j - 1, i] && !tmp) || (!trustTable[j - 1, i] && tmp);
                    tmp = trustTable[j - 1, i] && tmp;
                }
            }
           
        }
        public void FillFromFunction(string function)
        {
            
        }
    }
}
