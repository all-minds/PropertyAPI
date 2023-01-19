using PropertyAPI.Enums;
using PropertyAPI.Models;
using Google.Cloud.Firestore;

namespace PropertyAPI.Repositories;

public class PropertyRepository
{
    private readonly Collection _collection;
    public FirestoreDb _firestoreDb;

    public PropertyRepository(Collection collection)
    {
        _collection = collection;
        var firebaseAuthPath = "FirebaseAuth/carboncreditsfiap-firebase-adminsdk-dn8z4-162474e67b.json";
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", firebaseAuthPath);          
        _firestoreDb = FirestoreDb.Create("carboncreditsfiap");
    }

    public async Task<List<Property>> GetAllAsync()
    {
        Query query = _firestoreDb.Collection(_collection.ToString());
        var querySnapshot = await query.GetSnapshotAsync();
        var list = new List<Property>();

        foreach (var documentSnapshot in querySnapshot.Documents)
        {
            if (!documentSnapshot.Exists) continue;

            Property data = documentSnapshot.ConvertTo<Property>();

            if (data == null) continue;
                        
            data.Id = documentSnapshot.Id;
            list.Add(data);            
        }

        return list;
    }

    public async Task<Property?> GetAsync(Property entity)
    {
        var docRef = _firestoreDb.Collection(_collection.ToString()).Document(entity.Id);
        var documentSnapshot = await docRef.GetSnapshotAsync();

        if (documentSnapshot.Exists)
        {
            Property data = documentSnapshot.ConvertTo<Property>();
            data.Id = documentSnapshot.Id;
            return data;
        }

        return null;
    }

    public async Task<Property> AddAsync(Property entity)
    {
        var colRef = _firestoreDb.Collection(_collection.ToString());
        var doc = await colRef.AddAsync(entity);
        entity.Id = doc.Id;
        return entity;
    }

    public async Task<Property> UpdateAsync(Property entity)
    {
        var docRef = _firestoreDb.Collection(_collection.ToString()).Document(entity.Id);
        await docRef.SetAsync(entity, SetOptions.MergeAll);
        return entity;
    }

    public async Task DeleteAsync(Property entity)
    {
        var docRef = _firestoreDb.Collection(_collection.ToString()).Document(entity.Id);
        await docRef.DeleteAsync();
    }

    public async Task<List<Property>> QueryRecordAsync(Query query)
    {
        var querySnapshot = await query.GetSnapshotAsync();
        var list = new List<Property>();

        foreach (var documentSnapshot in querySnapshot.Documents)
        {
            if (!documentSnapshot.Exists) continue;

            Property data = documentSnapshot.ConvertTo<Property>();

            if (data == null) continue;
          
            data.Id = documentSnapshot.Id;
            list.Add(data);
        }

        return list;
    }

}