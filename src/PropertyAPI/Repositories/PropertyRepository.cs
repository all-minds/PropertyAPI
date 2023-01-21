using PropertyAPI.Enums;
using PropertyAPI.Models;
using Google.Cloud.Firestore;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using PropertyAPI.Services;

namespace PropertyAPI.Repositories;

public class PropertyRepository
{
    private readonly Collection _collection;
    public FirestoreDb _firestoreDb;
    public FirebaseApp _firebaseApp;
    public TokenService _tokenService;

    public PropertyRepository(Collection collection)
    {
        var firebaseAuthPath = "FirebaseAuth/carboncreditsfiap-firebase-adminsdk-dn8z4-162474e67b.json";
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", firebaseAuthPath);          

        _collection = collection;               
        _firestoreDb = FirestoreDb.Create("carboncreditsfiap");
        _tokenService = new TokenService();

        _firebaseApp = FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.GetApplicationDefault()
        });
    }

    public async Task<List<Property>> GetAllAsync(string idToken)
    {
        var uid = await _tokenService.GetUid(idToken);

        Query query = _firestoreDb.Collection(_collection.ToString());
        var querySnapshot = await query.GetSnapshotAsync();
        var list = new List<Property>();

        foreach (var documentSnapshot in querySnapshot.Documents)
        {
            if (!documentSnapshot.Exists) continue;

            Property data = documentSnapshot.ConvertTo<Property>();

            if (data == null) continue;            
            if (!(data.Owner_id == uid)) continue;

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

    public async Task<Property> AddAsync(Property entity, string idToken)
    {
        var uid = await _tokenService.GetUid(idToken);
        var colRef = _firestoreDb.Collection(_collection.ToString());
        entity.Owner_id = uid;
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