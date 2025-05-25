    using System;

    namespace E_commerce.Services.Interfaces
    {
        public static class Validator
        {
            public static bool CheckNull(object obj)
            {
                return obj == null;
            }

            public static bool CheckString(string str)
            {
                return string.IsNullOrWhiteSpace(str);
            }

            public static bool CheckNegativeOrZero(int value)
            {
                return value <= 0;
            }

            public static bool CheckNegativeOrZero(decimal value)
            {
                return value <= 0m;
            }

            public static bool CheckDuplicate(bool exists)
            {
                return exists;
            }

            public static bool CheckState(bool isValid)
            {
                return !isValid;
            }

            public static bool CheckOwnership(bool hasOwnership)
            {
                return !hasOwnership;
            }
        }
    }
