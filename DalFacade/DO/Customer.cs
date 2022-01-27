using System;


namespace DO
{
    /// <summary>
    /// Represent customer
    /// </summary>
    public struct Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// Return describe of Customer struct string
        /// </summary>
        /// <returns>describe of Customer struct string</returns>
        public override string ToString()
        {
            return $"Customer: Id: {Id}\n" +
                   $"Name: {Name}\n+" +
                   $"Phone: {Phone}\n" +
                   $"Longitude: {DalApi.IDal.SexagesimalPresentation(Longitude)}\n" +
                   $"Lattitude: {DalApi.IDal.SexagesimalPresentation(Latitude) }";
        }

    }
}