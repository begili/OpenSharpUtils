using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OpenSharpUtils.DataAccess.Core
{
    [Serializable]
    public class ResultSet<R1> : IEnumerable<R1>
    {
        public IEnumerable<R1> ResultSet1 { get; }

        public ResultSet(IEnumerable<R1> resultSet1)
        {
            this.ResultSet1 = resultSet1;
        }

        public IEnumerator<R1> GetEnumerator()
        {
            return ResultSet1.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ResultSet1.GetEnumerator();
        }
    }

    [Serializable]
    public class ResultSet<R1, R2>
    {
        public IEnumerable<R1> ResultSet1 { get; }

        public IEnumerable<R2> ResultSet2 { get; }

        public ResultSet(IEnumerable<R1> resultSet1, IEnumerable<R2> resultSet2)
        {
            this.ResultSet1 = resultSet1;
            this.ResultSet2 = resultSet2;
        }
    }

    [Serializable]
    public class ResultSet<R1, R2, R3>
    {
        public IEnumerable<R1> ResultSet1 { get; }

        public IEnumerable<R2> ResultSet2 { get; }

        public IEnumerable<R3> ResultSet3 { get; }

        public ResultSet(IEnumerable<R1> resultSet1, IEnumerable<R2> resultSet2, IEnumerable<R3> resultSet3)
        {
            this.ResultSet1 = resultSet1;
            this.ResultSet2 = resultSet2;
            this.ResultSet3 = resultSet3;
        }
    }
}
