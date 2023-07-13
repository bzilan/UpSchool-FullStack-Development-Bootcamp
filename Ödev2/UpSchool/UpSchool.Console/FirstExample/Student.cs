namespace UpSchool.Console.FirstExample
{
    public class Student : PersonBase, ITurkishPerson, IAge
    {
        public Student(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public Student()
        {
        }

        public int SchoolNumber { get; set; }
        public int Score1 { get; set; }
        public int Score2 { get; set; }
        public int Score3 { get; set; }
        public string TCID { get; set; }
        public int Age { get; set; }
       // public string FullName
       // {
        //    get
       //     {
       //        if (string.IsNullOrEmpty(FirstName)) //Empty ==""
        //        {
        //            return "İsimsiz";
        //        } 
         //      return $"{SchoolNumber} {FirstName} {LastName}";
         //   }
       // }
        public string FullName
        {
            get
            {
                return $"{SchoolNumber} {FirstName} {LastName}";
            }
            set
            {
                value = $"Şampiyon {FirstName} {LastName}";
            }
        }
        public int FinalNotes => (Score1 + Score2 + Score3) / 3;
       
        private int TotalScores()
        {
            return Score1 + Score2 + Score3;
        }
        public string GetFullName()
        {
            return $"{SchoolNumber} {FirstName} {LastName}";
        }

    }
}
