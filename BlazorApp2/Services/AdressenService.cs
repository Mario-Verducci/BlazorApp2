using BlazorApp2.Components;
using BlazorApp2.Data;
using BlazorApp2.Data.Entities;
using Microsoft.EntityFrameworkCore;
using SqliteWasmHelper;

namespace BlazorApp2.Services;

public class AdressenService(ISqliteWasmDbContextFactory<ApplicationDbContext> dbContextFactory)
{
    public async Task<List<Adresse>> GetAdressenAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        if (OnlineStatus.IsOnline)
        {
            //TODO: Adressen vom Server holen und lokal speichern
        }

        return await dbContext.Adressen.AsNoTracking().ToListAsync();
    }

    public async Task<Adresse> GetAdresseAsync(int id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        if (OnlineStatus.IsOnline)
        {

        }

        return await dbContext.Adressen.FindAsync(id);
    }

    public async Task<Adresse> AddAdresseAsync(Adresse adresse)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        adresse.Modified = true;
        if (OnlineStatus.IsOnline)
        {

            adresse.Modified = false;
        }

        dbContext.Adressen.Add(adresse);
        await dbContext.SaveChangesAsync();
        
        return adresse;
    }

    public async Task<Adresse> UpdateAdresseAsync(Adresse adresse)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        adresse.Modified = true;
        if (OnlineStatus.IsOnline)
        {

            adresse.Modified = false;
        }

        dbContext.Entry(adresse).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
        return adresse;
    }

    public async Task DeleteAdresseAsync(int id)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var adresse = await dbContext.Adressen.FindAsync(id);
        adresse.Deleted = true;

        if (OnlineStatus.IsOnline)
        {

            dbContext.Adressen.Remove(adresse);
            await dbContext.SaveChangesAsync();
        }

    }

    public async Task SyncAsync()
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        if (OnlineStatus.IsOnline)
        {
            //TODO: Alle Adressen die nicht synchronisiert sind an den Server senden und die Antwort verarbeiten

            var adressen = await dbContext.Adressen.Where(a => a.LastSynced == null).ToListAsync();
            foreach (var adresse in adressen)
            {
                //TODO: Adresse an den Server senden
                adresse.LastSynced = DateTime.Now;
            }

            await dbContext.SaveChangesAsync();
        }
    }
}