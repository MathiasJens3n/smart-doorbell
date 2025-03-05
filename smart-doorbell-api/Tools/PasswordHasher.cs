namespace smart_doorbell_api.Tools
{
    /// <summary>
    /// Provides methods for hashing and verifying passwords using the BCrypt algorithm.
    /// </summary>
    public static class PasswordHasher
    {
        /// <summary>
        /// Hashes the specified password using the BCrypt algorithm with a work factor of 12.
        /// </summary>
        /// <param name="password">The password to be hashed.</param>
        /// <returns>The hashed password as a string.</returns>
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        /// <summary>
        /// Verifies whether the entered password matches the stored hash using the BCrypt algorithm.
        /// </summary>
        /// <param name="enteredPassword">The plain-text password entered by the user.</param>
        /// <param name="storedHash">The hashed password stored in the system.</param>
        /// <returns>True if the entered password matches the stored hash; otherwise, false.</returns>
        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
        }
    }

}
