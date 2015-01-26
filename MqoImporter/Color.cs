namespace MqoImporter
{
    /// <summary>
    /// RGBを持つカラーの構造体
    /// </summary>
    public struct Color3
    {
        public float R;
        public float G;
        public float B;

        public override string ToString()
        {
            return R + ", " + G + ", " + B;
        }
    }

    /// <summary>
    /// RGBAを持つカラーの構造体
    /// </summary>
    public struct Color4
    {
        public float R;
        public float G;
        public float B;
        public float A;

        public override string ToString()
        {
            return R + ", " + G + ", " + B + ", " + A;
        }
    }
}
