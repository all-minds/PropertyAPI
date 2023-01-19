using Google.Cloud.Firestore;

namespace PropertyAPI.Models
{
    [FirestoreData]
    public class Property
    {
        [FirestoreDocumentId]
        public string? Id { get; set; }

        [FirestoreProperty]
        public string Owner_id { get; set; }

        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public string Street { get; set; }

        [FirestoreProperty]
        public string Number { get; set; }

        [FirestoreProperty]
        public string Complement { get; set; }

        [FirestoreProperty]
        public string City { get; set; }

        [FirestoreProperty]
        public string State { get; set; }

        [FirestoreProperty]
        public string? Zip_code { get; set; }

        [FirestoreProperty]
        public double Area { get; set; }

        [FirestoreProperty]
        public double Preserved_area { get; set; }    
    }
}