using Microsoft.EntityFrameworkCore;
using NyeriEvents.Data;
using System;

namespace NyeriEvents.Extensions
{
    public static class AddMigration
    {
        public static IApplicationBuilder ApplyMigration(this IApplicationBuilder app)
        {

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<EventDbContext>();
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }

            return app;
        }
    }
}
