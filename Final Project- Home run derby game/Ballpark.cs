    //creating a class of fields
    class ballpark
    {
        public string name;
        public string city;
        public int leftFieldDistance;
        public int rightFieldDistance;
        public int centerFieldDistance;

        public ballpark(string name, string city, int leftFieldDistance, int rightFieldDistance, int centerFieldDistance)
        {
            this.name = name;
            this.city = city;
            this.leftFieldDistance = leftFieldDistance;
            this.rightFieldDistance = rightFieldDistance;
            this.centerFieldDistance = centerFieldDistance;
        }
    }