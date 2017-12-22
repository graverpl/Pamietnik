using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamietnik
{
    internal abstract class Messages
    {
        public static string MandatoryFields()
        {
            return "Pola nie mogą być puste." + "\nWprowadź poprawne dane...";
        }

        public static string LoginError()
        {
            return "Niepoprawny login lub hasło." + "\nProszę popraw dane i spróbuj jeszcze raz...";
        }

        public static string PassConfirmationError()
        {
            return "Hasła nie są identyczne." + "\nSpróbuj jeszcze raz...";
        }

        public static string RegistrationCorrect()
        {
            return "Konto zostało założone. Możesz się już zalogować...";
        }

        public static string ConnectionError()
        {
            return "Błąd połączenia z bazą danych" + "\nProszę spróbować za chwilę...";
        }

        public static string UserExists()
        {
            return "Użytkownik już istnieje." + "\nZaloguj się lub spróbuj innej nazwy...";
        }

    }
}
