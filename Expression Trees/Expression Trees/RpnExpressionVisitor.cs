using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
namespace Expression_Trees
{
    public class RpnExpressionVisitor : ExpressionVisitor
    {
        // a stack to store the expressions
        Stack<Expression> expressions = new Stack<Expression>();
        //keep track of the root expression
        //Expression root = Expression.Constant(0);

        //constructor to create an instance of the class
        public RpnExpressionVisitor()
        {

        }
        /// <summary>
        /// Reads a string, splits it up by spaces, and then visits each string as a cosntant expression
        /// </summary>
        /// <param name="input"></param>
        public void ReadString(string input) {
            string[] stringExpressions = input.Split(' ');
            //for each of the strings
            foreach (string s in stringExpressions) {
                this.Visit(Expression.Constant(s));
            }
        }
        /// <summary>
        /// determines the node's type and then calls the specific method that will handle the given expression type.
        /// </summary>
        /// <param name="node">node to evaluate</param>
        /// <returns></returns>
        public override Expression Visit(Expression node)
        {   
            switch (node.NodeType) {
                case ExpressionType.Add:
                case ExpressionType.Subtract:
                case ExpressionType.Divide:
                case ExpressionType.Multiply:            
                case ExpressionType.Power:
                    return this.VisitBinary((BinaryExpression)node);
                case ExpressionType.Constant:
                    return this.VisitConstant((ConstantExpression)node);
                default:
                    return base.Visit(node);
            }

        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            expressions.Push(node);

            return node;
            
        }
        protected override Expression VisitConstant(ConstantExpression node)
        {
            var stringValue = node.Value.ToString();
            
            if (ExpressionTryBinary(stringValue, out var expressionType))
            {
                try
                {
                    var left = expressions.Pop();
                    var right = expressions.Pop();

                    return this.VisitBinary(Expression.MakeBinary(expressionType, left, right));
                }
                catch (InvalidOperationException)
                {
                    throw new InvalidOperationException("Operation could not execute. Too many operators.");
                }
            }
            else if (double.TryParse(stringValue, out double numberConstant))
            {

                expressions.Push(Expression.Constant(numberConstant));
                return expressions.Peek();
            }
            else
            {
                throw new Exception("Wrong format!! Must be a number or operators (+, -, /, *)");
            }
            
                       
        }

        protected bool ExpressionTryBinary(string expressionString, out ExpressionType expressionType){
            switch (expressionString) {
                case "+":
                    expressionType = ExpressionType.Add;
                    return true;
                case "-":
                    expressionType = ExpressionType.Subtract;
                    return true;
                case "*":
                    expressionType = ExpressionType.Multiply;
                    return true;
                case "/":
                    expressionType = ExpressionType.Divide;
                    return true;
                case "^":
                    expressionType = ExpressionType.Power;
                    return true;
                default:
                    expressionType = ExpressionType.Constant;
                    return false;                  
            }
        }


        public string  GetResult() {
            
            return Expression.Lambda(expressions.Peek()).Compile().DynamicInvoke().ToString();

        }

    }
}
