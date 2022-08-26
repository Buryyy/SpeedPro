namespace SpeedPro.Helpers
{
    public static class ScooterCommunicationUtil
    {
        public const string CONSTANT_SPEED_OFF = "FF551D010274";
        public const string CONSTANT_SPEED_ON = "FF551D010173";
        public const string ELECT_24V = "FF551B010171";
        public const string ELECT_36V = "FF551B010272";
        public const string ELECT_48V = "FF551B010373";
        public const string ELECT_60V = "FF551B010474";
        public const string GEAR_D1 = "FF551F010276";
        public const string GEAR_D2 = "FF551F010377";
        public const string GEAR_D3 = "FF551F010478";
        public const string GEAR_P = "FF551F010175";
        public const string LOCK = "FF551701026E";
        public const string SPEED_KM = "FF551801016E";
        public const string SPEED_MODE_ELECT = "FF5510010166";
        public const string SPEED_MODE_HELP = "FF5510010267";
        public const string SPEED_MODE_RIDE = "FF5510010368";
        public const string SPEED_MP = "FF551801026F";
        public const string START_MODE_NOT_ZERO = "FF551A010271";
        public const string START_MODE_ZERO = "FF551A010170";
        public const string UNLOCK = "FF551701016D";
        public const string WHEEL_10C = "FF551C010677";
        public const string WHEEL_12C = "FF551C010778";
        public const string WHEEL_14C = "FF551C010879";
        public const string WHEEL_16C = "FF551C01097A";
        public const string WHEEL_18C = "FF551C010A7B";
        public const string WHEEL_20C = "FF551C010B7C";
        public const string WHEEL_5C = "FF551C010172";
        public const string WHEEL_5_5C = "FF551C010273";
        public const string WHEEL_6C = "FF551C010374";
        public const string WHEEL_6_5C = "FF551C010475";
        public const string WHEEL_8C = "FF551C010576";

        public static byte[] HexStrToBytes(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        public static int GetHexVal(char hex)
        {
            int val = (int)hex;
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }
    }
}
