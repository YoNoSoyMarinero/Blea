using MimeKit.Encodings;
using server.Interfaces;

namespace server.Utilites
{
    /**
     * Class used for encoding and decoding tokens generated from Identitys framework.
     * Reason for this is the tokens in their native form can't go into the url
     */
    public class TokenEncoderUtility : ITokenEncoderUtility
    {
        public string EncodeToken(string token)
        {
            {
                string[] dirtyCharacters = { ";", "/", "?", ":", "@", "&", "=", "+", "$", "," };
                string[] cleanCharacters = { "p2n3t4G5l6m", "s1l2a3s4h", "q1e2st3i4o5n", "T22p14nt2s", "a9t", "a2n3nd", "e1q2ua88l", "p22l33u1ws", "d0l1ar5", "c0m8a1a" };

                foreach (string dirtyCharacter in dirtyCharacters)
                {
                    token = token.Replace(dirtyCharacter, cleanCharacters[Array.IndexOf(dirtyCharacters, dirtyCharacter)]);
                }
                return token;
            }
        }
        public string DecodeToken(string token)
        {
            string[] dirtyCharacters = { ";", "/", "?", ":", "@", "&", "=", "+", "$", "," };
            string[] cleanCharacters = { "p2n3t4G5l6m", "s1l2a3s4h", "q1e2st3i4o5n", "T22p14nt2s", "a9t", "a2n3nd", "e1q2ua88l", "p22l33u1ws", "d0l1ar5", "c0m8a1a" };
            foreach (string symbol in cleanCharacters)
            {
                token = token.Replace(symbol, dirtyCharacters[Array.IndexOf(cleanCharacters, symbol)]);
            }
            return token;
        }
    }
}
