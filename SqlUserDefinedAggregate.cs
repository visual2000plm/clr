using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Server;

namespace PLMCLRTools
{
    #region------  SqlUserDefinedAggregate --

    [Serializable]
    [SqlUserDefinedAggregate(

        Format.UserDefined,
        IsInvariantToNulls = true,
        IsInvariantToDuplicates = false,
        IsInvariantToOrder = false,
        MaxByteSize = -1)

    ]

    public class Concatenate : IBinarySerialize
    {
        // Intermediate result data of concatenation

        private StringBuilder _IntermediateResult;

        public void Init()
        {
            this._IntermediateResult = new StringBuilder();
        }

        // If the next value is not null, append it to the end of string

        public void Accumulate(SqlString value)
        {
            if (value.IsNull)
            {
                return;
            }

            this._IntermediateResult.Append(value.Value);
        }

        //Merges  Mutiple thread  aggegation  together the partial aggregate with this aggregate

        public void Merge(Concatenate part)
        {
            this._IntermediateResult.Append(part._IntermediateResult);
        }

        //Returns the result of the aggregation when finished

        public SqlString Terminate()
        {
            string result = string.Empty;

            if (this._IntermediateResult != null

                && this._IntermediateResult.Length > 0)
            {
                // bug is here
                //result = this.intermediateResult.ToString(0, this.intermediateResult.Length - 1);
                result = this._IntermediateResult.ToString(0, this._IntermediateResult.Length);
            }

            return new SqlString(result);
        }

        public void Read(BinaryReader reader)
        {
            _IntermediateResult = new StringBuilder(reader.ReadString());
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(this._IntermediateResult.ToString());
        }
    }

    [Serializable]
    [SqlUserDefinedAggregate(

        Format.UserDefined,
        IsInvariantToNulls = true,
        IsInvariantToDuplicates = false,
        IsInvariantToOrder = false,
        MaxByteSize = -1)
    ]
    public class ConcatenateWithSemicolon : IBinarySerialize
    {
        // Intermediate result data of concatenation

        private StringBuilder _IntermediateResult;

        public void Init()
        {
            this._IntermediateResult = new StringBuilder();
        }

        // If the next value is not null, append it to the end of string

        public void Accumulate(SqlString value)
        {
            if (value.IsNull)
            {
                return;
            }

            this._IntermediateResult.Append(value.Value + "; ");
        }

        //Merges the partial aggregate with this aggregate

        public void Merge(ConcatenateWithSemicolon part)
        {
            this._IntermediateResult.Append(part._IntermediateResult);
        }

        //Returns the result of the aggregation when finished

        public SqlString Terminate()
        {
            string result = string.Empty;

            if (this._IntermediateResult != null

                && this._IntermediateResult.Length > 0)
            {
                // bug is here
                // trucate last char
                result = this._IntermediateResult.ToString(0, this._IntermediateResult.Length - 2);
            }

            return new SqlString(result);
        }

        public void Read(BinaryReader reader)
        {
            _IntermediateResult = new StringBuilder(reader.ReadString());
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(this._IntermediateResult.ToString());
        }
    }

    [Serializable]
    [SqlUserDefinedAggregate(
        Format.UserDefined,
        IsInvariantToNulls = true,
        IsInvariantToDuplicates = false,
        IsInvariantToOrder = false,
        MaxByteSize = -1)
    ]
    public class FirstStringIsNotNull : IBinarySerialize
    {
        // Intermediate result data of concatenation

        private StringBuilder _IntermediateResult;

        public void Init()
        {
            this._IntermediateResult = new StringBuilder();
        }

        // If the next value is not null, append it to the end of string

        public void Accumulate(SqlString value)
        {
            if (value.IsNull)
            {
                return;
            }
            else if (_IntermediateResult.Length == 0)
            {
                this._IntermediateResult.Append(value.Value);
            }
        }

        //Merges the partial aggregate with this aggregate

        public void Merge(FirstStringIsNotNull part)
        {
            // this._IntermediateResult.Append(part._IntermediateResult);
        }

        //Returns the result of the aggregation when finished

        public SqlString Terminate()
        {
            string result = string.Empty;

            if (this._IntermediateResult != null

                && this._IntermediateResult.Length > 0)
            {
                // bug is here
                // trucate last char
                result = this._IntermediateResult.ToString(0, this._IntermediateResult.Length);
            }

            return new SqlString(result);
        }

        public void Read(BinaryReader reader)
        {
            _IntermediateResult = new StringBuilder(reader.ReadString());
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(this._IntermediateResult.ToString());
        }
    }

    [Serializable]
    [SqlUserDefinedAggregate(
        Format.UserDefined,
        IsInvariantToNulls = true,
        IsInvariantToDuplicates = false,
        IsInvariantToOrder = false,
        MaxByteSize = -1)
    ]
    public class FirstValue : IBinarySerialize
    {
        // Intermediate result data of concatenation

        private StringBuilder _IntermediateResult;

        public void Init()
        {
            this._IntermediateResult = new StringBuilder();
        }

        // If the next value is not null, append it to the end of string

        public void Accumulate(object value)
        {
            if (_IntermediateResult.Length == 0)
            {
                this._IntermediateResult.Append(value.ToString());
            }
        }

        //Merges the partial aggregate with this aggregate

        public void Merge(FirstValue part)
        {
            //this._IntermediateResult.Append(part._IntermediateResult);
        }

        //Returns the result of the aggregation when finished

        public object Terminate()
        {
            string result = string.Empty;

            if (this._IntermediateResult != null

                && this._IntermediateResult.Length > 0)
            {
                // bug is here
                // trucate last char
                result = this._IntermediateResult.ToString();
            }

            return result;
        }

        public void Read(BinaryReader reader)
        {
            _IntermediateResult = new StringBuilder(reader.ReadString());
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(this._IntermediateResult.ToString());
        }
    }

    [Serializable]
    [SqlUserDefinedAggregate(

        Format.UserDefined,
        IsInvariantToNulls = true,
        IsInvariantToDuplicates = false,
        IsInvariantToOrder = false,
        MaxByteSize = -1)
    ]
    public class DistinctConcatenateWithSemicolon : IBinarySerialize
    {
        // Intermediate result data of concatenation

        private StringBuilder _IntermediateResult;

        public void Init()
        {
            this._IntermediateResult = new StringBuilder();
        }

        // If the next value is not null, append it to the end of string

        public void Accumulate(SqlString value)
        {
            if (value.IsNull)
            {
                return;
            }

            string temp = this._IntermediateResult.ToString();

            this._IntermediateResult.Append(value.Value + "||");
        }

        //Merges the partial aggregate with this aggregate

        public void Merge(DistinctConcatenateWithSemicolon part)
        {
            this._IntermediateResult.Append(part._IntermediateResult);

            //  Accumulate(part.Terminate());
        }

        //public void Merge(CountVowels value)
        //{
        //    Accumulate(value.Terminate());
        //}

        //Returns the result of the aggregation when finished

        public class IMCaseInsensitiveComparer : IComparer
        {
            // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            int IComparer.Compare(Object x, Object y)
            {
                return ((new CaseInsensitiveComparer()).Compare(y, x));
            }
        }

        public SqlString Terminate()
        {
            string result = string.Empty;

            if (this._IntermediateResult != null

                && this._IntermediateResult.Length > 0)
            {
                string[] resultSort = this._IntermediateResult.ToString().Split("||".ToCharArray());
                for (int i = 0; i < resultSort.Length; i++)
                {
                    resultSort[i] = resultSort[i].Trim();
                }

                resultSort = resultSort.Distinct().ToArray();

                Array.Sort(resultSort);

                StringBuilder aToReturn = new StringBuilder();

                foreach (string s in resultSort)
                {
                    if (!string.IsNullOrEmpty(s))
                        aToReturn.Append(s + " ;");
                }
                if (aToReturn.Length > 0)
                {
                    aToReturn.Remove(aToReturn.Length - 2, 2);
                }

                result = aToReturn.ToString();
            }

            return new SqlString(result);
        }

        public void Read(BinaryReader reader)
        {
            _IntermediateResult = new StringBuilder(reader.ReadString());
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(this._IntermediateResult.ToString());
        }
    }

    //SELECT  count (distinct Blockid)  FROM pdmBlockSubitem

    [Serializable]
    [SqlUserDefinedAggregate(

        Format.UserDefined,
        IsInvariantToNulls = true,
        IsInvariantToDuplicates = false,
        IsInvariantToOrder = false,
        MaxByteSize = -1)
    ]
    public class DistinctCount : IBinarySerialize
    {
        // Intermediate result data of concatenation

        private StringBuilder _IntermediateResult;

        int _TotalCount = 0;

        public void Init()
        {
            this._IntermediateResult = new StringBuilder();
        }

        // If the next value is not null, append it to the end of string

        public void Accumulate(SqlString value)
        {
            if (value.IsNull)
            {
                return;
            }

            string temp = this._IntermediateResult.ToString();

            if (!temp.Contains(value.Value))
            {
                this._IntermediateResult.Append(value.Value + "; ");

                _TotalCount++;
            }
        }

        //Merges the partial aggregate with this aggregate
        //The Merge method can be used to merge another instance of this aggregate class with the current instance.????/
        //The Merge() method is used by the SQL engine itself
        //which might decide to split the aggregation work to multiple threads
        //and merge the results of which prior
        //to executing Terminate() and returning the result.

        // The Merge() method is called any time SQL decides to split up the workload,
        //process it on separate threads, and eventually merge all of the results.
        //It seems perfectly desirable to be able to maximize the use of computer resources.

        public void Merge(DistinctCount part)
        {
            // _TotalCount++;

            _TotalCount = _TotalCount + part._TotalCount;

            //  this._IntermediateResult.Append(part._IntermediateResult);
        }

        //Returns the result of the aggregation when finished

        public SqlString Terminate()
        {
            //string result = string.Empty;

            //if (this._IntermediateResult != null

            //    && this._IntermediateResult.Length > 0)
            //{
            //    // bug is here
            //    // trucate last char
            //    result = this._IntermediateResult.ToString(0, this._IntermediateResult.Length - 2);

            //}

            // return new SqlString(result);

            return _TotalCount.ToString();
        }

        public void Read(BinaryReader reader)
        {
            _IntermediateResult = new StringBuilder(reader.ReadString());
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(this._IntermediateResult.ToString());
        }
    }

    #endregion
}