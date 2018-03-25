namespace EastMed.Data.Migrations
{
    using Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using static CustomConfigEncrypt;

    internal sealed class Configuration : DbMigrationsConfiguration<EastMed.Data.Model.EastMedDB>
    {
       
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.Entity.MySqlMigrationSqlGenerator());
        }
        // Initialize the First admin 
        protected override void Seed(EastMed.Data.Model.EastMedDB context)
        {
            string EncryptionKey = "SHA512";
            string UserName = "Admin";
            string LastName = "Admin";
            string PasswordEnc = "eastmedAdmin";
            string UNIver_ID = "100000000";
            context.user.AddOrUpdate(u => u.UNI_ID,
                new user
                {
                     UNI_ID = UNIver_ID,
                    FIRST_NAME = UserName,
                    LAST_NAME = LastName,
                    PASSWORD = CustomEncrypt.passwordEncrypt(PasswordEnc, EncryptionKey),
                    TITLE = "Mr.Admin",
                    PHONE = "000000000",
                    FK_PRIVILEGE_ID = 5,
                    FK_LOCATION_ID = 0,
                    IsActive = true,
                    CREATED_DATE = DateTime.Now,
                    EMAIL = "Admin@eastmedadmin.com",
                });           
            context.SaveChanges();
        }
    }
}
