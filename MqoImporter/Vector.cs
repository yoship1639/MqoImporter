namespace MqoImporter
{
    /// <summary>
    /// XYを持つベクタ
    /// </summary>
    public struct Vector2
    {
        public float X;
        public float Y;

        public override string ToString()
        {
            return X + "," + Y;
        }
    }

    /// <summary>
    /// XYZを持つベクタ
    /// </summary>
    public struct Vector3
    {
        public float X;
        public float Y;
        public float Z;

        public override string ToString()
        {
            return X + ", " + Y + ", " + Z;
        }
    }

    /// <summary>
    /// XYZWを持つベクタ
    /// </summary>
    public struct Vector4
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public override string ToString()
        {
            return X + ", " + Y + ", " + Z + ", " + W;
        }
    }
}
