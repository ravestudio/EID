using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIDClient.Core.DataModel
{
    public class TokenModel
    {
        private static TokenModel _instance = null;

        public static TokenModel Instance
        {
            get
            {
                return _instance ?? new TokenModel();
            }
        }

        private object _robot = null;
        private object _analyst = null;

        public TokenModel()
        {
            _robot = "Robot";
            _analyst = "Analyst";
        }

        public object RobotToken()
        {
            return _robot;
        }

        public object AnalystToken()
        {
            return _analyst;
        }
    }
}
