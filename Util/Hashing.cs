namespace PasswordHashingDemo.Util
{
    public class Hashing
    {

        //generates a random salt for hashing with a complexity of 12
        private static string GetRandomSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(12);
        }

        //hashes a password using a randomly generated salt
        public static string HashPasword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, GetRandomSalt());
        }

        //validates a password against a hashed password
        public static bool ValidatePassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}