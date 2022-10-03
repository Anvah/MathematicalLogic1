using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MLTA_1
{
    public class Expression
    {
        string expression;
        public Expression(string ex)
        {
            expression = ex;
        }
        public string[] Operators = { "⊕","*", "∨", "∧", "¬", "→", "⟷" };
        public Queue<string> ToInvertPoland()
        {
            Stack<string> OperatorsStack = new Stack<string>();
            Queue<string> expessionQueue = new Queue<string>();
            Queue<string> invertPolandExpression = new Queue<string>();
            OperatorsStack.Clear();
            expessionQueue.Clear();
            invertPolandExpression.Clear();
            char[] exp = expression.ToCharArray();
            for (int i = 0; i < exp.Length; i++)
            {
                expessionQueue.Enqueue(exp[i].ToString());
            }
            while (expessionQueue.Count > 0)
            {
                if (Operators.Contains(expessionQueue.Peek()))
                {

                    if (OperatorsStack.Count > 0 && (((OperatorsStack.Peek() == "*" || OperatorsStack.Peek() == "∧") && !(expessionQueue.Peek() == "¬"))
                        || ((OperatorsStack.Peek() == "⊕" || OperatorsStack.Peek() == "∨") && (expessionQueue.Peek() == "⊕" || expessionQueue.Peek() == "∨" || expessionQueue.Peek() == "⟷" || expessionQueue.Peek() == "→"))
                        /*|| (OperatorsStack.Peek() == "¬" && expessionQueue.Peek() != "¬")*/
                        || (OperatorsStack.Peek() == "→" && (expessionQueue.Peek() == "⟷" || expessionQueue.Peek() == "→"))
                        || (OperatorsStack.Peek() == "⟷" && expessionQueue.Peek() == "⟷")))
                    {
                        
                        invertPolandExpression.Enqueue(OperatorsStack.Pop());
                    }
                    else if (OperatorsStack.Count > 0 && (OperatorsStack.Peek() == "¬" && expessionQueue.Peek() != "¬"))
                    {
                        while (OperatorsStack.Count > 0 && OperatorsStack.Peek() == "¬")
                        {
                            invertPolandExpression.Enqueue(OperatorsStack.Pop());
                        }
                    }
                    else
                    {
                        
                        OperatorsStack.Push(expessionQueue.Dequeue());
                    }
                    

                }
                else if (expessionQueue.Peek() == "(")
                {
                    OperatorsStack.Push(expessionQueue.Dequeue());
                }
                else if (expessionQueue.Peek() == ")")
                {
                    while (OperatorsStack.Peek() != "(")
                        invertPolandExpression.Enqueue(OperatorsStack.Pop());
                    OperatorsStack.Pop();
                    expessionQueue.Dequeue();
                }
                else if (expessionQueue.Peek() != "(" || expessionQueue.Peek() != ")")
                {
                    invertPolandExpression.Enqueue(expessionQueue.Dequeue());
                }
                else
                {
                    expessionQueue.Dequeue();
                }
                if (expessionQueue.Count == 0)
                {
                    while (OperatorsStack.Count > 0)
                        invertPolandExpression.Enqueue(OperatorsStack.Pop());
                }
            }
            return invertPolandExpression;
        }
        public bool Solve()
        {
            var invertPolandExpression = ToInvertPoland();
            Stack<bool> solution = new Stack<bool>();
            while (invertPolandExpression.Count > 0)
            {
                switch (invertPolandExpression.Peek())
                {
                    case "⊕":
                        {
                            invertPolandExpression.Dequeue();
                            bool a = solution.Pop();
                            bool b = solution.Pop();
                            solution.Push((a && !b) || (!a && b));
                            break;
                        }

                    case "*":
                        {
                            invertPolandExpression.Dequeue();
                            bool a = solution.Pop();
                            bool b = solution.Pop();
                            solution.Push(a && b);
                            break;
                        }
                    case "∧":
                        {
                            invertPolandExpression.Dequeue();
                            bool a = solution.Pop();
                            bool b = solution.Pop();
                            solution.Push(a && b);
                            break;
                        }
                    case "∨":
                        {
                            invertPolandExpression.Dequeue();
                            bool a = solution.Pop();
                            bool b = solution.Pop();
                            solution.Push(a || b);
                            break;
                        }
                    case "¬":
                        {
                            invertPolandExpression.Dequeue();
                            bool a = solution.Pop();
                            solution.Push(!a);
                            break;
                        }
                    case "→":
                        {
                            invertPolandExpression.Dequeue();
                            bool b = solution.Pop();
                            bool a = solution.Pop();    
                            solution.Push(!a || b);
                            break;
                        }
                    case "⟷":
                        {
                            invertPolandExpression.Dequeue();
                            bool b = solution.Pop();
                            bool a = solution.Pop();
                            solution.Push((!a || b) && (a || !b));
                            break;
                        }

                    default:
                        {
                            if (invertPolandExpression.Count > 0)
                            {
                                if (invertPolandExpression.Dequeue() == "1")
                                    solution.Push(true);
                                else
                                    solution.Push(false);
                            }
                                
                            break;
                        }


                }


            }
            return solution.Pop();
        }
        public string Simplification()
        {

            /*while (invertPolandExpression.Count > 0)
            {
                res += invertPolandExpression.Dequeue();
            }
            return res;*/
            var invertPolandExpression = ToInvertPoland();
            Stack<List<string>> solution = new Stack<List<string>>(100);
            while (invertPolandExpression.Count > 0)
            {
                switch (invertPolandExpression.Peek())
                {
                    case "⊕":
                        {
                            invertPolandExpression.Dequeue();
                            List<string> a = solution.Pop();
                            List<string> b = solution.Pop();
                            a = a.Concat(b).ToList();
                            solution.Push(a);
                            break;
                        }

                    case "*":
                        {
                            invertPolandExpression.Dequeue();
                            List<string> a = solution.Pop();
                            List<string> b = solution.Pop();
                            List<string> r = new List<string>();
                            for (int i = 0; i < a.Count; i++)
                            {
                                for (int j = 0; j < b.Count; j++)
                                {
                                    if (a[i] == "1")
                                        r.Add(b[j]);
                                    else if (b[j] == "1")
                                        r.Add(a[i]);
                                    else
                                        r.Add(a[i] +"*"+ b[j]);
                                }
                            }
                            solution.Push(r);
                            break;
                        }

                 

                    default:
                        {
                            if (invertPolandExpression.Count > 0)
                                solution.Push(new List<string> { invertPolandExpression.Dequeue() });
                            break;
                        }


                }


            }
            string answer = "";
            if(solution.Count > 0)
            {
                List<string> ans = solution.Pop();
                while (solution.Count > 0)
                {
                    Console.WriteLine(1234);
                    ans = ans.Concat(solution.Pop()).ToList();
                }

                List<string> ansS = new List<string>();
                for (int i = 0; i < ans.Count(); i++)
                {
                    if (ansS.Contains(ans[i]))
                        ansS.Remove(ans[i]);
                    else
                        ansS.Add(ans[i]);
                }
                /*for (int i = 0; i < ans.Count() - 1; i++)
                {
                    for (int j = i + 1; j < ans.Count(); j++)
                    {
                        if (ans[i] == ans[j])
                        {
                            string x = ans[i];
                            ans.Remove(x);
                            ans.Remove(x);
                        }
                    }

                }*/

                for (int i = 0; i < ansS.Count(); i++)
                {
                    answer += ansS[i];
                    if (i < ansS.Count() - 1)
                        answer += "⊕";
                }
                
            }
            return answer;
        }
           
    }
}
