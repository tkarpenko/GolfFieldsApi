////using Golf.Fields.Database;
////using Golf.Fields.Shared;
//using Microsoft.IdentityModel.Tokens;
////using System.Data.Entity;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;

//namespace Golf.Fields.Services
//{
//    public class SecurityServices
//    {

//        public static string JWT_KEY = "ju4tuiyn2316g9s0ob5tl8w5d38z11q3vqjsoc4zyqu95htg1lo5upoeracu";

//        private static SymmetricSecurityKey _secretKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(JWT_KEY));


//        /// <summary>
//        /// Auth by phone number
//        /// </summary>
//        /// <param name="phoneNumber"></param>
//        /// <returns></returns>
//        /// <exception cref="ArgumentNullException"></exception>
//        public AuthToken? Authenticate(string? phoneNumber)
//        {

//            if (string.IsNullOrWhiteSpace(phoneNumber))
//                throw new ArgumentNullException(nameof(phoneNumber));


//            AuthToken? token = new AuthToken();

//            try
//            {

//                phoneNumber = phoneNumber.Trim();

//                var user = GetUserByPhone(phoneNumber);

//                if (user != null &&
//                    !string.IsNullOrWhiteSpace(user.Phone))
//                {

//                    List<Claim> claims = new List<Claim>();
//                    claims.Add(new Claim(ClaimTypes.MobilePhone, user.Phone, ClaimValueTypes.String, "Golf"));


//                    var tokenJWT = new JwtSecurityToken(
//                        issuer: "Golf",
//                        audience: "Golf",
//                        claims: claims,
//                        notBefore: DateTime.UtcNow,
//                        expires: DateTime.UtcNow.AddDays(1),
//                        signingCredentials: new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256)
//                    );
//                    string jwtTokenStr = new JwtSecurityTokenHandler().WriteToken(tokenJWT);
//                    token.BearerToken = jwtTokenStr;
//                }

//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"{(ex.InnerException == null ? ex.Message : ex.InnerException.Message)}. {ex.StackTrace}");
//                throw;
//            }

//            return token;
//        }


//        public User? GetUserByPhone(string phone)
//        {
//            User? user = null;

//            using (var context = new GolfDbContext())
//            {
//                var phoneEncrypted = Encryption.GetSHA256HashData(phone);

//                if (context.Users != null)
//                {
//                    user = context.Users.SingleOrDefault(u => !string.IsNullOrWhiteSpace(u.Phone) && u.Phone.Equals(phoneEncrypted));

//                    if (user != null)
//                    {
//                        user.LastLoggedIn = DateTime.UtcNow;

//                        context.SaveChanges();
//                    }
//                }

//            }

//            return user;
//        }
//    }
//}

