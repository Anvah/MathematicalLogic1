using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLTA_1
{
    public class NormalFormConvertor
    {
        public TrustTable trustTable;
        public NormalFormConvertor(int count)
        {
            trustTable = new TrustTable(count);
        }
        public NormalFormConvertor(string func)
        {
            trustTable = new TrustTable(func);
        }
        public string SDNF(bool isDuality = false)
        {
            string d = "";
            bool[] res;
            if (!isDuality)
                res = trustTable.result;
            else
            {
                res = trustTable.dualityResult;
            }
            for (int i = 0; i < res.Length; i++)
            {
                if(res[i] == true)
                {
                    string dj = "(";
                    for (int j = 0; j < trustTable.varCount; j++)
                    {
                        if (trustTable.trustTable[i,j] == false)
                        {
                            dj += $"¬{trustTable.varArray[j]}";
                        }
                        else
                        {
                            dj += $"{trustTable.varArray[j]}";
                        }
                        if(j+1 < trustTable.varCount)
                        {
                            dj += "∧";
                        }
                    }
                    d += dj;
                    d += ")∨";
                }      
            }
            if (d.Length > 0)
            {
                d = d.Remove(d.Length - 1);
            }
            return d;
        }
        public string SKNF(bool isDuality = false)
        {
            bool[] res;
            if (!isDuality)
                res = trustTable.result;
            else
            {
                res = trustTable.dualityResult;
            }
            string k = "";
            for (int i = 0; i < res.Length; i++)
            {
                if (res[i] == false)
                {
                    string kj = "(";
                    for (int j = 0; j < trustTable.varCount; j++)
                    {
                        if (trustTable.trustTable[i, j] == true)
                        {
                            kj += $"¬{trustTable.varArray[j]}";
                        }
                        else
                        {
                            kj += $"{trustTable.varArray[j]}";
                        }
                        if (j + 1 < trustTable.varCount)
                        {
                            kj += "∨";
                        }
                    }
                    k += kj;
                    k += ")∧";
                }
            }
            if (k.Length > 0)
            {
                k = k.Remove(k.Length - 1);
            }
            return k;

        }
        public string SPNF(bool isDuality = false)
        {
            string spnf = "";
            List<string> element = new List<string>();
            string sdnf = SDNF(isDuality);
            for (int i = 0; i < sdnf.Length; i++)
            {
                if(sdnf[i].ToString() != "(" && sdnf[i].ToString() != ")" && sdnf[i].ToString() != "∧")
                {
                    if (sdnf[i].ToString() != "∨")
                    {
                        if (sdnf[i].ToString() == "¬")
                        {
                            spnf += $"(1⊕{sdnf[i+1]})";
                        }
                        else if (i == 0 || sdnf[i-1].ToString() != "¬")
                        {
                            spnf += sdnf[i];
                        }
                    }
                    else
                    {
                        spnf += "⊕";
                    }
                }
                else if(sdnf[i].ToString() == "∧")
                {
                    spnf += "*";
                }
            }
            Expression ex = new Expression(spnf);

            return ex.Simplification();

        }
    }
}
