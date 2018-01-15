using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamietnik
{
    internal abstract class Messages
    {
        internal static string MandatoryFields()
        {
            return "Pola nie mogą być puste." + "\nWprowadź poprawne dane...";
        }

        internal static string LoginError()
        {
            return "Niepoprawny login lub hasło." + "\nProszę popraw dane i spróbuj jeszcze raz...";
        }

        internal static string PassConfirmationError()
        {
            return "Hasła nie są identyczne." + "\nSpróbuj jeszcze raz...";
        }

        internal static string RegistrationCorrect()
        {
            return "Konto zostało założone. Możesz się już zalogować...";
        }

        internal static string ConnectionError()
        {
            return "Błąd połączenia z bazą danych" + "\nProszę spróbować za chwilę...";
        }

        internal static string UserExists()
        {
            return "Użytkownik już istnieje." + "\nZaloguj się lub spróbuj innej nazwy...";
        }

        internal static string GeneralError()
        {
            return "Wystąpił nieokreślony błąd." + "\nProszę spróbować za chwilę...";
        }
    }
}
