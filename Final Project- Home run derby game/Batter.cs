
 //creating a constructor and initializing the class Batter
    class Batter
    {
        public string name;
        public double power;
        public double contact;
        public double leftTendency;
        public double rightTendency;

        public Batter(string name, double power, double contact, double leftTendency, double rightTendency)
        {
            this.name = name;
            this.power = power;
            this.contact = contact;
            this.leftTendency = leftTendency;
            this.rightTendency = rightTendency;
        }
    }