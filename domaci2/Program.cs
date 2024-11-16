

using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Runtime.InteropServices.JavaScript;
using System.Transactions;

class Program
{
    public static Dictionary<int, (string NameOfUser, string SurnameOfUser, DateTime Date)> User =
        new Dictionary<int, (string, string, DateTime)>();
    public static Dictionary<int, (double CurrentAccount, double GiroAccount, double Prepaid)> UserAccounts = new Dictionary<int, (double, double, double)>();
    
    public static Dictionary<int, List<Dictionary<int, (double Amount, string TransactionDescription, string Type, string Category, DateTime Date)>>> CurrentAccTransactions = new Dictionary<int, List<Dictionary<int, (double, string, string, string, DateTime)>>>();
    public static Dictionary<int, List<Dictionary<int, (double Amount, string TransactionDescription, string Type, string Category, DateTime Date)>>> GiroAccTransactions = new Dictionary<int, List<Dictionary<int, (double, string, string, string, DateTime)>>>();
    public static Dictionary<int, List<Dictionary<int, (double Amount, string TransactionDescription, string Type, string Category, DateTime Date)>>> PrepaidAccTransactions = new Dictionary<int, List<Dictionary<int, (double, string, string, string, DateTime)>>>();
    
    public static Dictionary<int,(double Amount, string TransactionDescription, string Type, string Category, DateTime Date)> TransactionInfo = new Dictionary<int, (double, string, string, string, DateTime)>();
    public static List<Dictionary<int,(double Amount, string TransactionDescription, string Type, string Category, DateTime Date)>> TransactionList = new List<Dictionary<int, (double, string, string, string, DateTime)>>();
    
    static void Main()
    {
        
        Console.WriteLine("1 - Korisnici");
        Console.WriteLine("2 - Računi");
        Console.WriteLine("3 - Izlaz iz aplikacije");

        Console.Write("Odaberite željenu akciju: ");

        var firstAction = false;
        var numberOfFirstAction = 1;
        while (firstAction == false && numberOfFirstAction <= 1 || numberOfFirstAction >= 4)
        {
            firstAction = int.TryParse(Console.ReadLine(), out numberOfFirstAction);
            
        }

        switch (numberOfFirstAction)
        {
            case 1:
                Users();
                break;
            case 2:
                Accounts();
                break;
            case 3:
                Console.Clear();
                break;
        }
    }

    static void Users()
    {
        Console.Clear();
        Console.WriteLine("1 - Unos novog korisnika");
        Console.WriteLine("2 - Brisanje korisnika");
        Console.WriteLine("3 - Uređivanje korisnika");
        Console.WriteLine("4 - Pregled korisnika");
        Console.WriteLine("5 - Povratak");

        Console.Write("Odaberite željenu akciju: ");
        var secondAction = false;
        var numberOfSecondAction = 1;
        while (!secondAction && numberOfSecondAction <= 1 || numberOfSecondAction >= 6)
        {
            secondAction = int.TryParse(Console.ReadLine(), out numberOfSecondAction);
        }

        switch (numberOfSecondAction)
        {
            case 1:
                Console.Clear();
                InputUser();
                break;
            case 2:
                Console.Clear();
                DeleteUser();
                break;
            case 3:
                Console.Clear();
                UserEditing();
                break;
            case 4:
                Console.Clear();
                UserReview();
                break;
            case 5:
                Console.Clear();
                Main();
                break;
        }


    }

    
    static void InputUser()
    {

        Console.Write("Unesite ime: ");
        var NameOfUserInput = "";
        var Action = false;
        while (!Action)
        {
            NameOfUserInput = Console.ReadLine();
        }

        Console.Write("Unesite prezime: ");
        var SurnameOfUserInput = "";
        Action = false;
        while (!Action)
        {
            SurnameOfUserInput = Console.ReadLine();
        }

        Console.Write("Unesite id: ");
        var Id = 0;
        Action = false;
        while (!Action)
        {
            Action = int.TryParse(Console.ReadLine(), out Id);
            if (User.ContainsKey(Id))
            {
                Console.WriteLine("Id već postoji");
                Console.Write("Unesite ponovno id: ");
                Action = false;
                        
            }
        }
        

        Console.Write("Unesite datum rođenja: ");
        var Date = false;
        DateTime DateInput = DateTime.MinValue;
        while (!Date)
        {
            Date = DateTime.TryParse(Console.ReadLine(), out DateInput);
        }
        
        User.Add(Id, (NameOfUserInput, SurnameOfUserInput, DateInput));
        UserAccounts.Add(Id, (100.00, 0.00, 0.00));
        Console.Clear();
        Users();

    }

    static void DeleteUser()
    {
        Console.Clear();
        Console.WriteLine("Brisanje korisnika");
        Console.WriteLine("1 - po id-u");
        Console.WriteLine("2 - po imenu i prezimenu");
        Console.WriteLine("3 - Povratak ");

        Console.Write("Odaberite akciju: ");
        var Action = false;
        var typeOfAction = 1;
        while (!Action && typeOfAction <= 1 || typeOfAction >= 4)
        {   
            Action = int.TryParse(Console.ReadLine(), out typeOfAction);
        }

        switch (typeOfAction)
        {
            case 1:
                Console.Clear();
                DeleteById();
                break;

            case 2:
                Console.Clear();
                DeleteByName();
                break;

            case 3:
                Console.Clear();
                Users();
                break;
        }
    }

    static void DeleteById()
    {
        Console.WriteLine("Upišite id korisnika: ");
        var SelectedId = 0;
        var Check = false;
        var idExist = false;
        while (!Check && !idExist)
        {
            Check = int.TryParse(Console.ReadLine(), out SelectedId);
            idExist = User.ContainsKey(SelectedId);
            if (!idExist)
            {
                Console.Write("Uneseni id ne postoji, unesite ponovno: ");
            }
        }
        
        User.Remove(SelectedId);
        Console.WriteLine("Korisnik je uspiješno izbrisan");
        Console.Clear();
        Users();



    }

    static void DeleteByName()
    {
        Console.WriteLine("Upišite ime korisnika:");
        var SelectedName = "";
        var nameExist = false;
        var foundKey = -1;
        while (foundKey == -1)
        {
            SelectedName = Console.ReadLine();
            foreach (var kvp in User)
            {
                if (kvp.Value.NameOfUser == SelectedName)
                {
                    foundKey = kvp.Key;
                    User.Remove(foundKey);
                    Console.WriteLine("Korisnik je uspiješno izbrisan");
                    Console.Clear();
                    break; 
                }
                
            }

            if (foundKey == -1)
            {
                Console.WriteLine("Ime ne postoji, unesite ponovno: ");
            }
        }
            
        Users();
    }


    static void UserEditing()
    {
        Console.WriteLine("Uređivanje korisnika po id-u");
        Console.Write("Unesite id korisnika: ");
        var Check = false;
        var EditingId = 0;
        var idExist = false;
        while (!Check)
        {
            Check = int.TryParse(Console.ReadLine(), out EditingId);
            
            if (Check)
            {
                idExist = User.ContainsKey(EditingId);
                if (!idExist)
                {
                    Check = false;
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("1 - Promjena imena");
                    Console.WriteLine("2 - Promjena prezimena");
                    Console.WriteLine("3 - Promjena datuma");
                    Console.WriteLine("4 - Povratak");
                    
                    Console.WriteLine("Što bi ste htjeli promjeniti: ");
                    var typeOfChange = 1;
                    var Action = false;
                    while (!Action && typeOfChange <= 1 || typeOfChange >= 5)
                    {
                        Action = int.TryParse(Console.ReadLine(), out typeOfChange);
                    }

                    switch (typeOfChange)
                    {
                        case 1:
                            Console.Clear();
                            NameChange(EditingId);
                            break;
                        case 2:
                            SurnameChange(EditingId);
                            break;
                        case 3:
                            DateChange(EditingId);
                            break;
                        case 4:
                            Console.Clear();
                            Users();
                            break;
                            
                    }

                    static void NameChange(int EditingId)
                    {
                        Console.WriteLine("Upišite novo ime: ");
                        var newName = Console.ReadLine();
                        var existingUser = User[EditingId];
                        User[EditingId] = (newName, existingUser.SurnameOfUser, existingUser.Date);
                        Console.WriteLine(User[EditingId]);
                        UserEditing();

                    }

                    static void SurnameChange(int EditingId)
                    {
                        Console.WriteLine("Upišite novo prezime: ");
                        var newSurname = Console.ReadLine();
                        var existingUser = User[EditingId];
                        User[EditingId] = (existingUser.NameOfUser, newSurname, existingUser.Date);
                        Console.WriteLine(User[EditingId]);
                        UserEditing();
                    }

                    static void DateChange(int EditingId)
                    {
                        Console.WriteLine("Upišite novi datum: ");
                        var Check = false;
                        DateTime newDate = DateTime.MinValue;
                        
                        while (!Check)
                        {
                            Check = DateTime.TryParse(Console.ReadLine(), out newDate);
                        }
                        
                        var existingUser = User[EditingId];
                        User[EditingId] = (existingUser.NameOfUser, existingUser.SurnameOfUser, newDate);
                        Console.WriteLine(User[EditingId]);
                        UserEditing();
                    }
                    
                }
            }
            
        }
    }

    static void UserReview()
    {
        Console.Clear();
        Console.WriteLine("1 - ispis svih korisnika abecedno po prezimenu");
        Console.WriteLine("2 - ispis svih onih koji imaju iznad 30 godina");
        Console.WriteLine("3 - ispis svih onih koji imaju bar jedan račun u minusu");
        Console.WriteLine("4 - Povratak");

        Console.Write("Odaberite željenu akciju: ");
        var Action1 = false;
        var typeOfAction1 = 1;
        while (Action1 == false && typeOfAction1 <= 1 || typeOfAction1 >= 5)
        {
            Action1 = int.TryParse(Console.ReadLine(), out typeOfAction1);
        }

        switch (typeOfAction1)
        {
            case 1:
                Console.Clear();
                PrintUsersByName();
                break;
            case 2:
                Console.Clear();
                PrintUsersOver30();
                break;
            case 3:
                Console.Clear();
                PrintAccountsInMinus();
                break;
            case 4:
                Console.Clear();
                Users();
                break;
        }
    }

    static void PrintUsersByName()
    {
        Console.WriteLine("");
        var sortUser = User.OrderBy(name => name.Value.NameOfUser).ToList();

        foreach (var user in sortUser)
        {
            Console.WriteLine(user.Key +" - "+ user.Value.NameOfUser +" - "+ user.Value.SurnameOfUser +" - "+ user.Value.Date);
        }
        
        Console.Write("Upišite broj 1 za povratak: ");
        var Check = false;
        var back = 0;
        while (!Check && back != 1)
        {
            Check = int.TryParse(Console.ReadLine(), out back);
        }
        
        UserReview();
        
    }

    static void PrintUsersOver30()
    {   
        Console.WriteLine("");
        foreach (var dateUser in User)
        {
            DateTime date = dateUser.Value.Date;
            DateTime Today = DateTime.Today;
            var Years = Today.Year - date.Year;
            var Months = Today.Month - date.Month;
            var Days = Today.Day - date.Day;
            if (Days < 0)
            {
                Months -= 1;
            }

            if (Months < 0)
            {
                Years -= 1;
            }

            if (Years >= 30)
            {
                Console.WriteLine(dateUser.Key + " - " + dateUser.Value.NameOfUser + " - " + dateUser.Value.SurnameOfUser + " - " + dateUser.Value.Date);
            }
        }
        
        Console.Write("Upišite broj 1 za povratak: ");
        var Check = false;
        var back = 0;
        while (!Check && back != 1)
        {
            Check = int.TryParse(Console.ReadLine(), out back);
        }
        
        UserReview();
    }

    static void PrintAccountsInMinus()
    {
        foreach (var user in UserAccounts)
        {
            var BalanceCurrentAccount = user.Value.CurrentAccount;
            var BalanceGiroAccount = user.Value.GiroAccount;
            var BalancePrepaid = user.Value.Prepaid;

            if (BalancePrepaid < 0 || BalanceGiroAccount < 0 || BalanceCurrentAccount < 0)
            {
                var userInformations = User[user.Key]; 
                Console.WriteLine(user.Key +" - "+ userInformations.NameOfUser +" - "+ userInformations.SurnameOfUser +" - "+ userInformations.Date);
            }
        }
        Console.WriteLine("");
        Console.Write("Upišite broj 1 za povratak: ");
        var Check = false;
        var back = 0;
        while (!Check && back != 1)
        {
            Check = int.TryParse(Console.ReadLine(), out back);
        }
        
        UserReview();
        
    }
    
    static void Accounts()
    {   
        
        
        Console.Write("Upišite ime korisnika: ");
        var SelectedName = "";
        var nameExist = false;
        var foundKey = -1;
        while (foundKey == -1)
        {
            SelectedName = Console.ReadLine();
            foreach (var kvp in User)
            {
                if (kvp.Value.NameOfUser == SelectedName)
                {
                    foundKey = kvp.Key;
                    
                }
                
            }

            if (foundKey == -1)
            {
                Console.WriteLine("Ime ne postoji, unesite ponovno: ");
            }
        }
        
        
        
        Console.WriteLine("");
        Console.WriteLine("1 - Pregled");
        Console.WriteLine("0 - Povratak");
        
        Console.Write("Odaberite željenu akciju: ");
        var Action = false;
        var typeOfAction = 1;
        while (!Action && typeOfAction <= 0 || typeOfAction >= 2)
        {   
            Action = int.TryParse(Console.ReadLine(), out typeOfAction);
        }

        switch (typeOfAction)
        {
            case 1:
                Console.Clear();
                Console.WriteLine("1 - Tekući račun");
                Console.WriteLine("2 - Žiro račun");
                Console.WriteLine("3 - Prepaid");
                Console.WriteLine("4 - Povratak");
                
                Console.Write("Odaberite željeni račun: ");
                var Action1 = false;
                var typeOfAccount = 1;
                while (!Action1 && typeOfAccount <= 1 || typeOfAccount >= 5)
                {
                    Action1 = int.TryParse(Console.ReadLine(), out typeOfAccount);
                }

                switch (typeOfAccount)
                {
                    case 1:
                        Console.Clear();
                        TransactionManagment(typeOfAccount, foundKey);
                        break;
                    case 2:
                        Console.Clear();
                        TransactionManagment(typeOfAccount, foundKey);
                        break;
                    case 3:
                        Console.Clear();
                        TransactionManagment(typeOfAccount, foundKey);
                        break;
                    case 4 :
                        Console.Clear();
                        Accounts();
                        break;
                }
                break;
            
            case 0:
                Console.Clear();
                Main();
                break;
        }

        static void TransactionManagment(int typeOfAccount, int foundKey)
        {
            
            Console.WriteLine("1 - Unos nove transakcije");
            Console.WriteLine("2 - Brisanje transakcije");
            Console.WriteLine("3 - Uređivanje transakcija");
            Console.WriteLine("4 - Pregled transakcija");
            Console.WriteLine("5 - Financijsko izvješće");
            Console.WriteLine("6 - Povratak");
            
            Console.WriteLine("");
            Console.Write("Odaberite željenu akciju: ");
            var Action = false;
            var typeOfAction = 1;
            while (!Action && typeOfAction <= 1 || typeOfAction >= 7)
            {   
                Action = int.TryParse(Console.ReadLine(), out typeOfAction);
            }

            switch (typeOfAction)
            {
                case 1:
                    Console.Clear();
                    TransactionEntry(typeOfAccount, foundKey);
                    break;
                case 2:
                    Console.Clear();
                    DeleteTransaction(typeOfAccount, foundKey);
                    break;
                case 3:
                    Console.Clear();
                    TransactionEdit(typeOfAccount, foundKey);
                    break;
                case 4:
                    Console.Clear();
                    ViewInTransactions(typeOfAccount, foundKey);
                    break;
                case 5:
                    Console.Clear();
                    FinancialReport(typeOfAccount, foundKey);
                    break;
                case 6:
                    Console.Clear();
                    Accounts();
                    break;
            }

            static void TransactionEntry(int typeOfAccount, int foundKey)
            {
                
                Console.WriteLine("1 - Trenutno izvršena transkacija");
                Console.WriteLine("2 - Ranije izvršena transakcija");
                
                Console.Write("Unesite broj: ");
                var Action = false;
                var typeOfAction = 1;
                while (!Action && typeOfAction <= 1 || typeOfAction >= 3)
                {
                    Action = int.TryParse(Console.ReadLine(), out typeOfAction);
                }
                Console.Clear();
                
                DateTime TimeOfTransaction = DateTime.Now;
                
                if (typeOfAction == 2)
                {
                    Action = false;
                    while (!Action)
                    {
                        Console.Write("Unesite datum transakcije: ");
                        Action = DateTime.TryParse(Console.ReadLine(), out TimeOfTransaction);
                    }
                }
                
                
                Console.Write("Unesite id transakcije: ");
                var Action1 = false;
                var TransactionId = 0;
                while (!Action1)
                {
                    Action1 = int.TryParse(Console.ReadLine(), out TransactionId);
                    if (TransactionInfo.ContainsKey(TransactionId))
                    {
                        Console.WriteLine("Transakcija već postoji pod tim id-om");
                        Console.Write("Unesite ponovno id: ");
                        Action1 = false;
                        
                    }
                }
                
                Console.Write("Unesite iznos transakcije: ");
                var Action2 = false;
                var TransactionAmount = 0.00;
                while (!Action2)
                {
                    Action2 = double.TryParse(Console.ReadLine(), out TransactionAmount);
                }
                
                Console.Write("Unesite opis transakcije: ");
                var TransactionDescription = "";
                while (TransactionDescription != "standarna transakcija")
                {
                    TransactionDescription = Console.ReadLine();
                }
                
                Console.Write("Unesite tip transakcije: ");
                var TransactionType = "";
                while (TransactionType != "prihod" && TransactionType != "rashod")
                {
                    TransactionType = Console.ReadLine();
                }
                
                Console.Write("Unesite kategoriju transakcije: ");
                var TransactionCategory = "";
                
                if(TransactionType == "prihod")
                {
                    string[] arrayIncome = {"plaća", "honorar", "poklon"};
                    var Check = 0;
                    while (Check == 0)
                    {
                        TransactionCategory = Console.ReadLine();
                        for (int i = 0; i < arrayIncome.Length; i++)
                        {
                            if (arrayIncome[i] == TransactionCategory)
                            {
                                Check = 1;
                                break;
                            }
                        }
                    }
                }
                
                else
                {
                    string[] arrayExpense = { "hrana", "prijevoz", "sport" };
                    var Check = 0;
                    while (Check == 0)
                    {
                        TransactionCategory = Console.ReadLine();
                        for (int i = 0; i < arrayExpense.Length; i++)
                        {
                            if (arrayExpense[i] == TransactionCategory)
                            {
                                Check = 1;
                                break;
                            }
                        }
                    }
                }
               
                TransactionInfo.Add(TransactionId,(TransactionAmount, TransactionDescription, TransactionType, TransactionCategory, TimeOfTransaction));
                
                TransactionList.Add(TransactionInfo);
                
                switch (typeOfAccount)
                {
                    case 1:
                        CurrentAccTransactions[foundKey] = TransactionList;
                        if (TransactionInfo[TransactionId].Type == "prihod")
                        {
                            var Amount = UserAccounts[foundKey].CurrentAccount;
                            var dictionary = UserAccounts[foundKey];
                            UserAccounts[foundKey] = (Amount+TransactionAmount, dictionary.GiroAccount, dictionary.Prepaid);
                        }
                        else
                        {
                            var Amount = UserAccounts[foundKey].CurrentAccount;
                            var dictionary = UserAccounts[foundKey];
                            UserAccounts[foundKey] = (Amount-TransactionAmount, dictionary.GiroAccount, dictionary.Prepaid);
                        }
                        break;
                    case 2:
                        GiroAccTransactions[foundKey] = TransactionList;
                        if (TransactionInfo[TransactionId].Type == "prihod")
                        {
                            var Amount = UserAccounts[foundKey].GiroAccount;
                            var dictionary = UserAccounts[foundKey];
                            UserAccounts[foundKey] = (dictionary.CurrentAccount, Amount+TransactionAmount, dictionary.Prepaid);
                        }
                        else
                        {
                            var Amount = UserAccounts[foundKey].GiroAccount;
                            var dictionary = UserAccounts[foundKey];
                            UserAccounts[foundKey] = (dictionary.CurrentAccount, Amount-TransactionAmount, dictionary.Prepaid);
                        }
                        break;
                    case 3:
                        PrepaidAccTransactions[foundKey] = TransactionList;
                        if (TransactionInfo[TransactionId].Type == "prihod")
                        {
                            var Amount = UserAccounts[foundKey].Prepaid;
                            var dictionary = UserAccounts[foundKey];
                            UserAccounts[foundKey] = (dictionary.CurrentAccount, dictionary.GiroAccount, Amount + TransactionAmount);
                        }
                        else
                        {
                            var Amount = UserAccounts[foundKey].Prepaid;
                            var dictionary = UserAccounts[foundKey];
                            UserAccounts[foundKey] = (dictionary.CurrentAccount, dictionary.GiroAccount, Amount - TransactionAmount);
                        }
                        break;
                }
                
                Console.Clear();
                TransactionManagment(typeOfAccount, foundKey);

            }   

            static void DeleteTransaction(int typeOfAccount, int foundKey)
            {
                Console.WriteLine("1 - po id-u");
                Console.WriteLine("2 - ispod unesenog iznosa");
                Console.WriteLine("3 - iznad unesenog iznosa");
                Console.WriteLine("4 - svih prihoda");
                Console.WriteLine("5 - svih rashoda");
                Console.WriteLine("6 - svih transakcija za odabranu kategoriju");
                Console.WriteLine("7 - Povratak");
                
                Console.Write("Odaberite željenu akciju: ");
                var Action = false;
                var typeOfAction = 1;
                while (!Action && typeOfAction <= 1 || typeOfAction >= 8)
                {
                    Action = int.TryParse(Console.ReadLine(), out typeOfAction);
                }

                switch (typeOfAction)
                {
                    case 1:
                        Console.Clear();
                        DeleteTansactionId(typeOfAccount, foundKey);
                        break;
                    case 2:
                        Console.Clear();
                        UnderAmount(typeOfAccount, foundKey);
                        break;
                    case 3:
                        Console.Clear();
                        AboveAmount(typeOfAccount, foundKey);
                        break;
                    case 4:
                        Console.Clear();
                        DelRevenues(typeOfAccount, foundKey);
                        break;
                    case 5:
                        Console.Clear();
                        DelExpenses(typeOfAccount, foundKey);
                        break;
                    case 6:
                        Console.Clear();
                        DeleteChategory(typeOfAccount, foundKey);
                        break;
                    case 7:
                        Console.Clear();
                        TransactionManagment(typeOfAccount, foundKey);
                        break;
                }

                static void DeleteTansactionId(int typeOfAccount, int foundKey)
                {
                    switch (typeOfAccount)
                    {
                        case 1:
                            var list = CurrentAccTransactions[foundKey];
                            Console.Write("Unesite id transakcije: ");
                            var Action = false;
                            var DelTransactionIdD = 0;
                            var Exist = 0;
                            while (!Action)
                            {
                                Action = int.TryParse(Console.ReadLine(), out DelTransactionIdD );
                            }

                            for (int i = 0; i < list.Count; i++)
                            {
                                var dictionary = list[i];
                                
                                foreach (var key in dictionary.Keys)
                                {
                                    if (key == DelTransactionIdD)
                                    {
                                        list.RemoveAt(i);
                                        Console.WriteLine("Transakcija je uspiješno izbrisana");
                                        Console.Write("1 - Povratak: ");
                                        Action = false;
                                        var one = 0;
                                        while (!Action && one != 1)
                                        {
                                            Action = int.TryParse(Console.ReadLine(), out one);
                                        }
                                        DeleteTransaction(typeOfAccount, foundKey);
                                        Exist += 1;
                                        break;  
                                    }
                                }
                               
                            }

                            if (Exist == 0)
                            {
                                Console.WriteLine("Id transakcije ne postoji");
                                DeleteTansactionId(typeOfAccount, foundKey);
                            }
                            Console.Clear();
                            DeleteTransaction(typeOfAccount, foundKey);
                            break;
                        
                        case 2:
                            list = GiroAccTransactions[foundKey];
                            Console.Write("Unesite id transakcije: ");
                            Action = false;
                            DelTransactionIdD = 0;
                            Exist = 0;
                            while (!Action)
                            {
                                Action = int.TryParse(Console.ReadLine(), out DelTransactionIdD );
                            }

                            for (int i = 0; i < list.Count; i++)
                            {
                                var dictionary = list[i];
                                
                                foreach (var key in dictionary.Keys)
                                {
                                    if (key == DelTransactionIdD)
                                    {
                                        list.RemoveAt(i);
                                        Console.WriteLine("Transakcija je uspiješno izbrisana");
                                        Console.Write("1 - Povratak: ");
                                        Action = false;
                                        var one = 0;
                                        while (!Action && one != 1)
                                        {
                                            Action = int.TryParse(Console.ReadLine(), out one);
                                        }
                                        DeleteTransaction(typeOfAccount, foundKey);
                                        Exist += 1;
                                        break;  
                                    }
                                }
                               
                            }

                            if (Exist == 0)
                            {
                                Console.WriteLine("Id transakcije ne postoji");
                                DeleteTansactionId(typeOfAccount, foundKey);
                            }
                            Console.Clear();
                            DeleteTransaction(typeOfAccount, foundKey);
                            break;
                        
                        case 3:
                            list = PrepaidAccTransactions[foundKey];
                            Console.Write("Unesite id transakcije: ");
                            Action = false;
                            DelTransactionIdD = 0;
                            Exist = 0;
                            while (!Action)
                            {
                                Action = int.TryParse(Console.ReadLine(), out DelTransactionIdD );
                            }

                            for (int i = 0; i < list.Count; i++)
                            {
                                var dictionary = list[i];
                                
                                foreach (var key in dictionary.Keys)
                                {
                                    if (key == DelTransactionIdD)
                                    {
                                        list.RemoveAt(i);
                                        Console.WriteLine("Transakcija je uspiješno izbrisana");
                                        Console.Write("1 - Povratak: ");
                                        Action = false;
                                        var one = 0;
                                        while (!Action && one != 1)
                                        {
                                            Action = int.TryParse(Console.ReadLine(), out one);
                                        }
                                        DeleteTransaction(typeOfAccount, foundKey);
                                        Exist += 1;
                                        break;  
                                    }
                                }
                               
                            }

                            if (Exist == 0)
                            {
                                Console.WriteLine("Id transakcije ne postoji");
                                DeleteTansactionId(typeOfAccount, foundKey);
                            }
                            Console.Clear();
                            DeleteTransaction(typeOfAccount, foundKey);
                            break;
                    }
                }
                
                static void UnderAmount(int typeOfAccount, int foundKey)
                {
                    switch (typeOfAccount)
                    {
                        case 1:
                            var list = CurrentAccTransactions[foundKey];
                            Console.Write("Unesite iznos: ");
                            
                            var Action = false;
                            double DelAmount = 0;
                            while (!Action)
                            {
                                Action = double.TryParse(Console.ReadLine(), out DelAmount );
                            }

                            for (int i = 0; i < list.Count; i++)
                            {   
                                var dictionary = list[i];
                                
                                foreach (var key in dictionary.Keys)
                                {
                                    var Amount = dictionary[key].Amount;
                                    if (DelAmount >= Amount)
                                    {
                                        list.Remove(list[i]);
                                    }
                                    
                                }
                                
                               
                            }
                            Console.WriteLine("Transakcija je uspiješno izbrisana");
                            Console.Write("1 - Povratak: ");
                            Action = false;
                            var one = 0;
                            while (!Action && one != 1)
                            {
                                Action = int.TryParse(Console.ReadLine(), out one);
                            }
                            DeleteTransaction(typeOfAccount, foundKey);
                            break;
                        case 2:
                            list = GiroAccTransactions[foundKey];
                            Console.Write("Unesite iznos: ");
                            
                            Action = false;
                            DelAmount = 0;
                            while (!Action)
                            {
                                Action = double.TryParse(Console.ReadLine(), out DelAmount );
                            }

                            for (int i = 0; i < list.Count; i++)
                            {   
                                var dictionary = list[i];
                                foreach (var key in dictionary.Keys)
                                {
                                    var Amount = dictionary[key].Amount;
                                    if (DelAmount >= Amount)
                                    {
                                        list.Remove(list[i]);
                                    }
                                    
                                }
                            }
                            Console.WriteLine("Transakcija je uspiješno izbrisana");
                            Console.Write("1 - Povratak: ");
                            Action = false;
                            one = 0;
                            while (!Action && one != 1)
                            {
                                Action = int.TryParse(Console.ReadLine(), out one);
                            }
                            DeleteTransaction(typeOfAccount, foundKey);
                            break;
                        case 3:
                            list = PrepaidAccTransactions[foundKey];
                            Console.Write("Unesite iznos: ");
                            
                            Action = false;
                            DelAmount = 0;
                            while (!Action)
                            {
                                Action = double.TryParse(Console.ReadLine(), out DelAmount );
                            }

                            for (int i = 0; i < list.Count; i++)
                            {   
                                var dictionary = list[i];
                                foreach (var key in dictionary.Keys)
                                {
                                    var Amount = dictionary[key].Amount;
                                    if (DelAmount >= Amount)
                                    {
                                        list.Remove(list[i]);
                                    }
                                    
                                }
                            }
                            Console.WriteLine("Transakcija je uspiješno izbrisana");
                            Console.Write("1 - Povratak: ");
                            Action = false;
                            one = 0;
                            while (!Action && one != 1)
                            {
                                Action = int.TryParse(Console.ReadLine(), out one);
                            }
                            DeleteTransaction(typeOfAccount, foundKey);
                            break;
                    }
                }
                
                static void AboveAmount(int typeOfAccount, int foundKey)
                {
                    switch (typeOfAccount)
                    {
                        case 1:
                            var list = CurrentAccTransactions[foundKey];
                            Console.Write("Unesite iznos: ");
                            
                            var Action = false;
                            double DelAmount = 0;
                            while (!Action)
                            {
                                Action = double.TryParse(Console.ReadLine(), out DelAmount );
                            }

                            for (int i = 0; i < list.Count; i++)
                            {   
                                var dictionary = list[i];
                                foreach (var key in dictionary.Keys)
                                {
                                    var Amount = dictionary[key].Amount;
                                    if (DelAmount <= Amount)
                                    {
                                        list.Remove(list[i]);
                                    }
                                    
                                }
                            }
                            Console.WriteLine("Transakcija je uspiješno izbrisana");
                            Console.Write("1 - Povratak: ");
                            Action = false;
                            var one = 0;
                            while (!Action && one != 1)
                            {
                                Action = int.TryParse(Console.ReadLine(), out one);
                            }
                            DeleteTransaction(typeOfAccount, foundKey);
                            break;
                        
                        case 2:
                            list = GiroAccTransactions[foundKey];
                            Console.Write("Unesite iznos: ");
                            
                            Action = false;
                            DelAmount = 0;
                            while (!Action)
                            {
                                Action = double.TryParse(Console.ReadLine(), out DelAmount );
                            }

                            for (int i = 0; i < list.Count; i++)
                            {   
                                var dictionary = list[i];
                                foreach (var key in dictionary.Keys)
                                {
                                    var Amount = dictionary[key].Amount;
                                    if (DelAmount <= Amount)
                                    {
                                        list.Remove(list[i]);
                                    }
                                    
                                }
                            }
                            Console.WriteLine("Transakcija je uspiješno izbrisana");
                            Console.Write("1 - Povratak: ");
                            Action = false;
                            one = 0;
                            while (!Action && one != 1)
                            {
                                Action = int.TryParse(Console.ReadLine(), out one);
                            }
                            DeleteTransaction(typeOfAccount, foundKey);
                            break;
                        
                        case 3:
                            list = PrepaidAccTransactions[foundKey];
                            Console.Write("Unesite iznos: ");
                            
                            Action = false;
                            DelAmount = 0;
                            while (!Action)
                            {
                                Action = double.TryParse(Console.ReadLine(), out DelAmount );
                            }

                            for (int i = 0; i < list.Count; i++)
                            {   
                                var dictionary = list[i];
                                foreach (var key in dictionary.Keys)
                                {
                                    var Amount = dictionary[key].Amount;
                                    if (DelAmount <= Amount)
                                    {
                                        list.Remove(list[i]);
                                    }
                                    
                                }
                            }
                            Console.WriteLine("Transakcija je uspiješno izbrisana");
                            Console.Write("1 - Povratak: ");
                            Action = false;
                            one = 0;
                            while (!Action && one != 1)
                            {
                                Action = int.TryParse(Console.ReadLine(), out one);
                            }
                            DeleteTransaction(typeOfAccount, foundKey);
                            break;
                    }
                }
                
                static void DelRevenues(int typeOfAccount, int foundKey)
                {
                    switch (typeOfAccount)
                    {
                        case 1:
                            var list = CurrentAccTransactions[foundKey];

                            for (int i = 0; i < list.Count; i++)
                            {   
                                var dictionary = list[i];
                                foreach (var key in dictionary.Keys)
                                {
                                    var Type = dictionary[key].Type;
                                    if (Type == "plaća" || Type == "honorar" || Type == "poklon")
                                    {
                                        list.Remove(list[i]);
                                    }
                                    
                                }
                                
                            }
                            Console.WriteLine("Transakcija je uspiješno izbrisana");
                            Console.Write("1 - Povratak: ");
                            var Action = false;
                            var one = 0;
                            while (!Action && one != 1)
                            {
                                Action = int.TryParse(Console.ReadLine(), out one);
                            }
                            DeleteTransaction(typeOfAccount, foundKey);
                            break;
                        case 2:
                            list = GiroAccTransactions[foundKey];

                            for (int i = 0; i < list.Count; i++)
                            {   
                                var dictionary = list[i];
                                foreach (var key in dictionary.Keys)
                                {
                                    var Type = dictionary[key].Type;
                                    if (Type == "plaća" || Type == "honorar" || Type == "poklon")
                                    {
                                        list.Remove(list[i]);
                                    }
                                    
                                }
                            }
                            Console.WriteLine("Transakcija je uspiješno izbrisana");
                            Console.Write("1 - Povratak: ");
                            Action = false;
                            one = 0;
                            while (!Action && one != 1)
                            {
                                Action = int.TryParse(Console.ReadLine(), out one);
                            }
                            DeleteTransaction(typeOfAccount, foundKey);
                            break;
                        case 3:
                            list = PrepaidAccTransactions[foundKey];

                            for (int i = 0; i < list.Count; i++)
                            {   
                                var dictionary = list[i];
                                foreach (var key in dictionary.Keys)
                                {
                                    var Type = dictionary[key].Type;
                                    if (Type == "plaća" || Type == "honorar" || Type == "poklon")
                                    {
                                        list.Remove(list[i]);
                                    }
                                    
                                }
                            }
                            Console.WriteLine("Transakcija je uspiješno izbrisana");
                            Console.Write("1 - Povratak: ");
                            Action = false;
                            one = 0;
                            while (!Action && one != 1)
                            {
                                Action = int.TryParse(Console.ReadLine(), out one);
                            }
                            DeleteTransaction(typeOfAccount, foundKey);
                            break;
                    }
                }
                
                static void DelExpenses(int typeOfAccount, int foundKey)
                {
                    switch (typeOfAccount)
                    {
                        case 1:
                            var list = CurrentAccTransactions[foundKey];

                            for (int i = 0; i < list.Count; i++)
                            {   
                                var dictionary = list[i];
                                foreach (var key in dictionary.Keys)
                                {
                                    var Type = dictionary[key].Type;
                                    if (Type == "hrana" || Type == "prijevoz" || Type == "sport")
                                    {
                                        list.Remove(list[i]);
                                    }
                                    
                                }
                            }
                            Console.WriteLine("Transakcija je uspiješno izbrisana");
                            Console.Write("1 - Povratak: ");
                            var Action = false;
                            var one = 0;
                            while (!Action && one != 1)
                            {
                                Action = int.TryParse(Console.ReadLine(), out one);
                            }
                            DeleteTransaction(typeOfAccount, foundKey);
                            break;
                        
                        case 2:
                            list = GiroAccTransactions[foundKey];

                            for (int i = 0; i < list.Count; i++)
                            {   
                                var dictionary = list[i];
                                foreach (var key in dictionary.Keys)
                                {
                                    var Type = dictionary[key].Type;
                                    if (Type == "hrana" || Type == "prijevoz" || Type == "sport")
                                    {
                                        list.Remove(list[i]);
                                    }
                                    
                                }
                            }
                            Console.WriteLine("Transakcija je uspiješno izbrisana");
                            Console.Write("1 - Povratak: ");
                            Action = false;
                            one = 0;
                            while (!Action && one != 1)
                            {
                                Action = int.TryParse(Console.ReadLine(), out one);
                            }
                            DeleteTransaction(typeOfAccount, foundKey);
                            break;
                        
                        case 3:
                            list = PrepaidAccTransactions[foundKey];

                            for (int i = 0; i < list.Count; i++)
                            {   
                                var dictionary = list[i];
                                foreach (var key in dictionary.Keys)
                                {
                                    var Type = dictionary[key].Type;
                                    if (Type == "hrana" || Type == "prijevoz" || Type == "sport")
                                    {
                                        list.Remove(list[i]);
                                    }
                                    
                                }
                            }
                            Console.WriteLine("Transakcija je uspiješno izbrisana");
                            Console.Write("1 - Povratak: ");
                            Action = false;
                            one = 0;
                            while (!Action && one != 1)
                            {
                                Action = int.TryParse(Console.ReadLine(), out one);
                            }
                            DeleteTransaction(typeOfAccount, foundKey);
                            break;
                    }
                }
                
                static void DeleteChategory(int typeOfAccount, int foundKey)
                {
                    switch (typeOfAccount)
                    {
                        case 1:
                            var list = CurrentAccTransactions[foundKey];
                            Console.Write("Unesite kategoriju transakcije: ");
                            var Category = "";
                            while (Category != "plaća" || Category != "honorar" || Category != "poklon" || Category != "sport" || Category != "hrana" || Category != "prijevoz")
                            {
                                Category = Console.ReadLine();
                            }

                            for (int i = 0; i < list.Count; i++)
                            {   
                                var dictionary = list[i];
                                
                                foreach (var key in dictionary.Keys)
                                {
                                    var Category1 = dictionary[key].Category;
                                    if (Category1 == Category)
                                    {
                                        list.Remove(list[i]);
                                    }
                                    
                                }
                                
                            }
                            Console.WriteLine("Transakcija je uspiješno izbrisana");
                            Console.Write("1 - Povratak: ");
                            var Action = false;
                            var one = 0;
                            while (!Action && one != 1)
                            {
                                Action = int.TryParse(Console.ReadLine(), out one);
                            }
                            DeleteTransaction(typeOfAccount, foundKey);
                            break;
                        case 2:
                            list = GiroAccTransactions[foundKey];
                            Console.Write("Unesite kategoriju transakcije: ");
                            Category = "";
                            while (Category != "plaća" || Category != "honorar" || Category != "poklon" || Category != "sport" || Category != "hrana" || Category != "prijevoz")
                            {
                                Category = Console.ReadLine();
                            }

                            for (int i = 0; i < list.Count; i++)
                            {   
                                var dictionary = list[i];
                                foreach (var key in dictionary.Keys)
                                {
                                    var Category1 = dictionary[key].Category;
                                    if (Category1 == Category)
                                    {
                                        list.Remove(list[i]);
                                    }
                                    
                                }
                            }
                            Console.WriteLine("Transakcija je uspiješno izbrisana");
                            Console.Write("1 - Povratak: ");
                            Action = false;
                            one = 0;
                            while (!Action && one != 1)
                            {
                                Action = int.TryParse(Console.ReadLine(), out one);
                            }
                            DeleteTransaction(typeOfAccount, foundKey);
                            break;
                        
                        case 3:
                            list = PrepaidAccTransactions[foundKey];
                            Console.Write("Unesite kategoriju transakcije: ");
                            Category = "";
                            while (Category != "plaća" || Category != "honorar" || Category != "poklon" || Category != "sport" || Category != "hrana" || Category != "prijevoz")
                            {
                                Category = Console.ReadLine();
                            }

                            for (int i = 0; i < list.Count; i++)
                            {   
                                var dictionary = list[i];
                                foreach (var key in dictionary.Keys)
                                {
                                    var Category1 = dictionary[key].Category;
                                    if (Category1 == Category)
                                    {
                                        list.Remove(list[i]);
                                    }
                                    
                                }
                            }
                            Console.WriteLine("Transakcija je uspiješno izbrisana");
                            Console.Write("1 - Povratak: ");
                            Action = false;
                            one = 0;
                            while (!Action && one != 1)
                            {
                                Action = int.TryParse(Console.ReadLine(), out one);
                            }
                            DeleteTransaction(typeOfAccount, foundKey);
                            break;
                    }
                }
                
                
            }

            static void TransactionEdit(int typeOfAccount, int foundKey)
            {
                Console.WriteLine("Uređivanje transakcije po id-u");
                Console.WriteLine("Ako želite povratak upišite: -1");
                Console.WriteLine("");
                Console.Write("Upišite id transakcije (ili -1 za izlaz): ");
               

                switch (typeOfAccount)
                {
                    case 1:
                        var list = CurrentAccTransactions[foundKey];
                        var Action = false;
                        var EditTransaction = 0;
                        var Exist = 0;
                        while (!Action)
                        {
                            Action = int.TryParse(Console.ReadLine(), out EditTransaction );
                            if (EditTransaction == -1)
                            {
                                TransactionManagment(typeOfAccount, foundKey);
                                break;
                            }
                        }

                        for (int i = 0; i < list.Count; i++)
                        {
                            var dictionary = list[i];

                            foreach (var key in dictionary.Keys)
                            {
                                if (key == EditTransaction)
                                {
                                    Console.WriteLine("1 - Promjenite iznos");
                                    Console.WriteLine("2 - Promjenite kategoriju");
                                    Console.WriteLine("3 - Promjenite tip");
                                    Console.WriteLine("4 - Provratak");

                                    Console.Write("Odaberite što želite promjeniti: ");
                                    var Action1 = false;
                                    var numberOfAction = 1;
                                    while (!Action1 && numberOfAction <= 1 || numberOfAction >= 5)
                                    {
                                        Action1 = int.TryParse(Console.ReadLine(), out numberOfAction);
                                    }

                                    switch (numberOfAction)
                                    {
                                        case 1:
                                            Console.Write("Odaberite novi iznos: ");
                                            var Action2 = false;
                                            var newAmount = 0;
                                            while (!Action2)
                                            {
                                                Action2 = int.TryParse(Console.ReadLine(), out newAmount);

                                            }

                                            list[i][key] = (newAmount, dictionary[key].TransactionDescription,
                                                dictionary[key].Type,
                                                dictionary[key].Category, dictionary[key].Date);
                                            TransactionEdit(typeOfAccount, foundKey);
                                            break;
                                        case 2:
                                            Console.Write("Odaberite novi opis: ");

                                            if (dictionary[key].Type == "prihod")
                                            {
                                                var Action4 = false;
                                                var word = "";
                                                while (word != "hrana" || word != "prijevoz" || word != "sport")
                                                {
                                                    word = Console.ReadLine();
                                                }

                                                list[i][key] = (dictionary[key].Amount,
                                                    dictionary[key].TransactionDescription, dictionary[key].Type,
                                                    word, dictionary[key].Date);
                                                TransactionEdit(typeOfAccount, foundKey);
                                            }

                                            else
                                            {
                                                var Action4 = false;
                                                var word = "";
                                                while (word != "plaća" || word != "honorar" || word != "poklon")
                                                {
                                                    word = Console.ReadLine();
                                                }

                                                list[i][key] = (dictionary[key].Amount,
                                                    dictionary[key].TransactionDescription, dictionary[key].Type,
                                                    word, dictionary[key].Date);
                                                TransactionEdit(typeOfAccount, foundKey);
                                            }

                                            break;
                                        case 3:
                                            if (dictionary[key].Type == "rashod")
                                            {
                                                Console.WriteLine("Tip transakcije je promijenjen u prihOd");
                                                list[i][key] = (dictionary[key].Amount,
                                                    dictionary[key].TransactionDescription, "prihod",
                                                    dictionary[key].Category, dictionary[key].Date);
                                                TransactionEdit(typeOfAccount, foundKey);
                                            }
                                            else
                                            {
                                                Console.WriteLine("Tip transakcije je promijenjen u rashod");
                                                list[i][key] = (dictionary[key].Amount,
                                                    dictionary[key].TransactionDescription, "rashod",
                                                    dictionary[key].Category, dictionary[key].Date);
                                                TransactionEdit(typeOfAccount, foundKey);
                                            }

                                            break;
                                        case 4:
                                            Console.Clear();
                                            TransactionEdit(typeOfAccount, foundKey);
                                            break;
                                    }
                                    Exist += 1;
                                }
                            }
                            
                            
                            
                        }

                        if (Exist == 0)
                        {
                            Console.WriteLine("Id transakcije ne postoji");
                            TransactionEdit(typeOfAccount, foundKey);
                        }
                        break;
                    case 2:
                        list = GiroAccTransactions[foundKey];
                        Action = false;
                        EditTransaction = 0;
                        Exist = 0;
                        while (!Action)
                        {
                            Action = int.TryParse(Console.ReadLine(), out EditTransaction );
                        }

                        for (int i = 0; i < list.Count; i++)
                        {
                            var dictionary = list[i];

                            foreach (var key in dictionary.Keys)
                            {
                                if (key == EditTransaction)
                                {
                                    Console.WriteLine("1 - Promjenite iznos");
                                    Console.WriteLine("2 - Promjenite kategoriju");
                                    Console.WriteLine("3 - Promjenite tip");
                                    Console.WriteLine("4 - Provratak");
                                    
                                    Console.Write("Odaberite što želite promjeniti: ");
                                    var Action1 = false;
                                    var numberOfAction = 1;
                                    while (!Action1 && numberOfAction <= 1 || numberOfAction >= 5)
                                    {
                                        Action1 = int.TryParse(Console.ReadLine(), out numberOfAction);
                                    }

                                    switch (numberOfAction)
                                    {
                                        case 1:
                                            Console.Write("Odaberite novi iznos: ");
                                            var Action2 = false;
                                            var newAmount = 0;
                                            while (!Action2)
                                            {
                                                Action2 = int.TryParse(Console.ReadLine(), out newAmount);
                                            }

                                            list[i][key] = (newAmount, dictionary[key].TransactionDescription, dictionary[key].Type,
                                                dictionary[key].Category, dictionary[key].Date);
                                            TransactionEdit(typeOfAccount, foundKey);
                                            break;
                                        case 2:
                                            Console.Write("Odaberite novi opis: ");
                                            
                                            if (dictionary[key].Type == "prihod")
                                            {
                                                var Action4 = false;
                                                var word = "";
                                                while (word != "hrana" || word != "prijevoz" || word != "sport")
                                                {
                                                    word = Console.ReadLine();
                                                }
                                                
                                                list[i][key] = (dictionary[key].Amount, dictionary[key].TransactionDescription, dictionary[key].Type,
                                                    word, dictionary[key].Date);
                                                TransactionEdit(typeOfAccount, foundKey);
                                            }
                                            
                                            else
                                            {
                                                var Action4 = false;
                                                var word = "";
                                                while (word != "plaća" || word != "honorar" || word != "poklon")
                                                {
                                                    word = Console.ReadLine();
                                                }
                                                
                                                list[i][key] = (dictionary[key].Amount, dictionary[key].TransactionDescription, dictionary[key].Type,
                                                    word, dictionary[key].Date);
                                                TransactionEdit(typeOfAccount, foundKey);
                                            }
                                            break;
                                        case 3:
                                            if (dictionary[key].Type == "rashod")
                                            {
                                                Console.WriteLine("Tip transakcije je promijenjen u prihdd");
                                                list[i][key] = (dictionary[key].Amount, dictionary[key].TransactionDescription, "prihod",
                                                dictionary[key].Category, dictionary[key].Date);
                                                TransactionEdit(typeOfAccount, foundKey);
                                            }
                                            else
                                            {
                                                Console.WriteLine("Tip transakcije je promijenjen u rashod");
                                                list[i][key] = (dictionary[key].Amount, dictionary[key].TransactionDescription, "rashod",
                                                    dictionary[key].Category, dictionary[key].Date);
                                                TransactionEdit(typeOfAccount, foundKey);
                                            }
                                            break;
                                        case 4:
                                            Console.Clear();
                                            TransactionEdit(typeOfAccount, foundKey);
                                            break;
                                    }
                                    Exist += 1;
                                }
                            }
                            
                               
                        }

                        if (Exist == 0)
                        {
                            Console.WriteLine("Id transakcije ne postoji");
                            TransactionEdit(typeOfAccount, foundKey);
                        }
                        break;
                    
                    case 3:
                        list = PrepaidAccTransactions[foundKey];
                        Action = false;
                        EditTransaction = 0;
                        Exist = 0;
                        while (!Action)
                        {
                            Action = int.TryParse(Console.ReadLine(), out EditTransaction );
                        }

                        for (int i = 0; i < list.Count; i++)
                        {
                            var dictionary = list[i];
                            foreach (var key in dictionary.Keys)
                            {
                                if (key == EditTransaction)
                                {
                                    Console.WriteLine("1 - Promjenite iznos");
                                    Console.WriteLine("2 - Promjenite kategoriju");
                                    Console.WriteLine("3 - Promjenite tip");
                                    Console.WriteLine("4 - Provratak");
                                    
                                    Console.Write("Odaberite što želite promjeniti: ");
                                    var Action1 = false;
                                    var numberOfAction = 1;
                                    while (!Action1 && numberOfAction <= 1 || numberOfAction >= 5)
                                    {
                                        Action1 = int.TryParse(Console.ReadLine(), out numberOfAction);
                                    }

                                    switch (numberOfAction)
                                    {
                                        case 1:
                                            Console.Write("Odaberite novi iznos: ");
                                            var Action2 = false;
                                            var newAmount = 0;
                                            while (!Action2)
                                            {
                                                Action2 = int.TryParse(Console.ReadLine(), out newAmount);
                                            }

                                            list[i][key] = (newAmount, dictionary[key].TransactionDescription, dictionary[key].Type, dictionary[key].Category, dictionary[key].Date);
                                            TransactionEdit(typeOfAccount, foundKey);
                                            break;
                                        case 2:
                                            Console.Write("Odaberite novi opis: ");
                                            
                                            if (dictionary[key].Type == "prihod")
                                            {
                                                var Action4 = false;
                                                var word = "";
                                                while (word != "hrana" || word != "prijevoz" || word != "sport")
                                                {
                                                    word = Console.ReadLine();
                                                }
                                                
                                                list[i][key] = (dictionary[key].Amount, dictionary[key].TransactionDescription, dictionary[key].Type,
                                                    word, dictionary[key].Date);
                                                TransactionEdit(typeOfAccount, foundKey);
                                            }
                                            
                                            else
                                            {
                                                var Action4 = false;
                                                var word = "";
                                                while (word != "plaća" || word != "honorar" || word != "poklon")
                                                {
                                                    word = Console.ReadLine();
                                                }
                                                
                                                list[i][key] = (dictionary[key].Amount, dictionary[key].TransactionDescription, dictionary[key].Type,
                                                    word, dictionary[key].Date);
                                                TransactionEdit(typeOfAccount, foundKey);
                                            }
                                            break;
                                        case 3:
                                            if (dictionary[key].Type == "rashod")
                                            {
                                                Console.WriteLine("Tip transakcije je promijenjen u prihdd");
                                                list[i][key] = (dictionary[key].Amount, dictionary[key].TransactionDescription, "prihod",
                                                dictionary[key].Category, dictionary[key].Date);
                                                TransactionEdit(typeOfAccount, foundKey);
                                            }
                                            else
                                            {
                                                Console.WriteLine("Tip transakcije je promijenjen u rashod");
                                                list[i][key] = (dictionary[key].Amount, dictionary[key].TransactionDescription, "rashod",
                                                    dictionary[key].Category, dictionary[key].Date);
                                                TransactionEdit(typeOfAccount, foundKey);
                                            }
                                            break;
                                        case 4:
                                            Console.Clear();
                                            TransactionEdit(typeOfAccount, foundKey);
                                            break;
                                    }
                                
                                    Exist += 1;
                                }
                            }
                               
                        }

                        if (Exist == 0)
                        {
                            Console.WriteLine("Id transakcije ne postoji");
                            TransactionEdit(typeOfAccount, foundKey);
                        }
                        break;
                    
                }
                
                
            }

            static void ViewInTransactions(int typeOfAccount, int foundKey)
            {
                Console.WriteLine("1 - Ispis svih transakcija");
                Console.WriteLine("2 - Ispis svih transakcija sortirane uzlazno");
                Console.WriteLine("3 - Ispis svih transakcija sortiranih silazno");
                Console.WriteLine("4 - Ispis svih transakcija po opisu abecedno");
                Console.WriteLine("5 - Ispis svih transakcija po datumu ulazno");
                Console.WriteLine("6 - Ispis svih transakcija po datumu silazno");
                Console.WriteLine("7 - Ispis svih prihoda");
                Console.WriteLine("8 - Ispis svih rashoda");
                Console.WriteLine("9 - Ispis svih transakcija za odabranu kategoriju");
                Console.WriteLine("10 - Ispis svih transakcija za odabrani tip i kategoriju");
                Console.WriteLine("11 - Povratak");
                
                Console.Write("Odaberite željenu akciju: ");
                var Action = false;
                var numberOfAction = 1;
                while (!Action && numberOfAction <= 1 || numberOfAction >= 12)
                {
                    Action = int.TryParse(Console.ReadLine(), out numberOfAction);
                }

                switch (numberOfAction)
                {
                    case 1:
                        TransactionPrint(typeOfAccount, foundKey);
                        break;
                    case 2:
                        TransactionAscending(typeOfAccount, foundKey);
                        break;
                    case 3:
                        TransactionDownward(typeOfAccount, foundKey);
                        break;
                    case 4:
                        TransactionAlphabet(typeOfAccount, foundKey);
                        break;
                    case 5:
                        TransactionDateAscending(typeOfAccount, foundKey);
                        break;
                    case 6:
                        TransactionDateDownward(typeOfAccount, foundKey);
                        break;
                    case 7:
                        TransactionIncome(typeOfAccount, foundKey);
                        break;
                    case 8:
                        TransactionExpense(typeOfAccount, foundKey);
                        break;
                    case 9:
                        TransactionCategory(typeOfAccount, foundKey);
                        break;
                    case 10:
                        TransactionTypeCategory(typeOfAccount, foundKey);
                        break;
                    case 11:
                        Console.Clear();
                        TransactionManagment(typeOfAccount, foundKey);
                        break;
                    
                }
                
            }

            static void TransactionPrint(int typeOfAccount, int foundKey)
            {
                switch (typeOfAccount)
                {
                    case 1:
                        var list = CurrentAccTransactions[foundKey];
                        
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            foreach (var key in dictionary.Keys)
                            {
                                Console.WriteLine(dictionary[key].Type +" - "+ dictionary[key].Amount +" - "+ dictionary[key].TransactionDescription +" - "+ dictionary[key].Category +" - "+ dictionary[key].Date);
                            }
                            
                        }
                        Console.Write("1 - Povratak");
                        var Action = false;
                        var one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }
                        ViewInTransactions(typeOfAccount, foundKey);
                        
                        break;
                    case 2:
                        list = GiroAccTransactions[foundKey];
                        
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            
                            var dictionary = list[i];
                            foreach (var key in dictionary.Keys)
                            {
                                Console.WriteLine(dictionary[key].Type +" - "+ dictionary[key].Amount +" - "+ dictionary[key].TransactionDescription +" - "+ dictionary[key].Category +" - "+ dictionary[key].Date);
                            }
                        }
                        Console.Write("1 - Povratak");
                        Action = false;
                        one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }
                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                    
                    case 3:
                        list = PrepaidAccTransactions[foundKey];
                        
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            foreach (var key in dictionary.Keys)
                            {
                                Console.WriteLine(dictionary[key].Type +" - "+ dictionary[key].Amount +" - "+ dictionary[key].TransactionDescription +" - "+ dictionary[key].Category +" - "+ dictionary[key].Date);
                            }
                        }
                        Console.Write("1 - Povratak");
                        Action = false;
                        one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }
                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                }
            }
            
            static void TransactionAscending(int typeOfAccount, int foundKey)
            {
                switch (typeOfAccount)
                {
                    case 1:
                        var list = CurrentAccTransactions[foundKey];

                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            var sortTransaction = dictionary.OrderBy(amount => amount.Value.Amount).ToList();

                            foreach (var transaction in sortTransaction)
                            {
                                Console.WriteLine(transaction.Value.Type +" - " + transaction.Value.Amount + " - " + transaction.Value.Type + " - " + transaction.Value.Category + " - "+ transaction.Value.Date);
                            }
                        }
                        
                        Console.Write("1 - Povratak");
                        var Action = false;
                        var one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }
                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                    case 2:
                        list = GiroAccTransactions[foundKey];

                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            var sortTransaction = dictionary.OrderBy(amount => amount.Value.Amount).ToList();

                            foreach (var transaction in sortTransaction)
                            {
                                Console.WriteLine(transaction.Value.Type +" - " + transaction.Value.Amount + " - " + transaction.Value.Type + " - " + transaction.Value.Category + " - "+ transaction.Value.Date);
                            }
                        }
                        
                        Console.Write("1 - Povratak");
                        Action = false;
                        one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }
                        
                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                    case 3:
                        list = PrepaidAccTransactions[foundKey];

                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            var sortTransaction = dictionary.OrderBy(amount => amount.Value.Amount).ToList();

                            foreach (var transaction in sortTransaction)
                            {
                                Console.WriteLine(transaction.Value.Type +" - " + transaction.Value.Amount + " - " + transaction.Value.Type + " - " + transaction.Value.Category + " - "+ transaction.Value.Date);
                            }
                        }
                        
                        Console.Write("1 - Povratak");
                        Action = false;
                        one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }
                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                }
            }
            
            static void TransactionDownward(int typeOfAccount, int foundKey)
            {
                switch (typeOfAccount)
                {
                    case 1:
                        var list = CurrentAccTransactions[foundKey];

                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            var sortTransaction = dictionary.OrderByDescending(amount => amount.Value.Amount).ToList();

                            foreach (var transaction in sortTransaction)
                            {
                                Console.WriteLine(transaction.Value.Type +" - " + transaction.Value.Amount + " - " + transaction.Value.Type + " - " + transaction.Value.Category + " - "+ transaction.Value.Date);
                            }
                        }
                        
                        Console.Write("1 - Povratak");
                        var Action = false;
                        var one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }
                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                    case 2:
                        list = GiroAccTransactions[foundKey];

                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            var sortTransaction = dictionary.OrderByDescending(amount => amount.Value.Amount).ToList();

                            foreach (var transaction in sortTransaction)
                            {
                                Console.WriteLine(transaction.Value.Type +" - " + transaction.Value.Amount + " - " + transaction.Value.Type + " - " + transaction.Value.Category + " - "+ transaction.Value.Date);
                            }
                        }
                        
                        Console.Write("1 - Povratak");
                        Action = false;
                        one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }
                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                    case 3:
                        list = PrepaidAccTransactions[foundKey];

                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            var sortTransaction = dictionary.OrderByDescending(amount => amount.Value.Amount).ToList();

                            foreach (var transaction in sortTransaction)
                            {
                                Console.WriteLine(transaction.Value.Type +" - " + transaction.Value.Amount + " - " + transaction.Value.Type + " - " + transaction.Value.Category + " - "+ transaction.Value.Date);
                            }
                        }
                        
                        Console.Write("1 - Povratak");
                        Action = false;
                        one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }
                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                }
            }
            
            static void TransactionAlphabet(int typeOfAccount, int foundKey)
            {
                switch (typeOfAccount)
                {
                    case 1:
                        var list = CurrentAccTransactions[foundKey];

                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            var sortTransaction = dictionary.OrderBy(amount => amount.Value.TransactionDescription).ToList();

                            foreach (var transaction in sortTransaction)
                            {
                                Console.WriteLine(transaction.Value.Type +" - " + transaction.Value.Amount + " - " + transaction.Value.Type + " - " + transaction.Value.Category + " - "+ transaction.Value.Date);
                            }
                        }
                        
                        Console.Write("1 - Povratak");
                        var Action = false;
                        var one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }

                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                    case 2:
                        list = GiroAccTransactions[foundKey];

                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            var sortTransaction = dictionary.OrderBy(amount => amount.Value.TransactionDescription).ToList();

                            foreach (var transaction in sortTransaction)
                            {
                                Console.WriteLine(transaction.Value.Type +" - " + transaction.Value.Amount + " - " + transaction.Value.Type + " - " + transaction.Value.Category + " - "+ transaction.Value.Date);
                            }
                        }
                        
                        Console.Write("1 - Povratak");
                        Action = false;
                        one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }
                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                    case 3:
                        list = PrepaidAccTransactions[foundKey];

                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            var sortTransaction = dictionary.OrderBy(amount => amount.Value.TransactionDescription).ToList();

                            foreach (var transaction in sortTransaction)
                            {
                                Console.WriteLine(transaction.Value.Type +" - " + transaction.Value.Amount + " - " + transaction.Value.Type + " - " + transaction.Value.Category + " - "+ transaction.Value.Date);
                            }
                        }
                        
                        Console.Write("1 - Povratak");
                        Action = false;
                        one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }
                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                }
            }
            
            static void TransactionDateAscending(int typeOfAccount, int foundKey)
            {
                switch (typeOfAccount)
                {
                    case 1:
                        var list = CurrentAccTransactions[foundKey];

                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            var sortTransaction = dictionary.OrderBy(amount => amount.Value.Date).ToList();

                            foreach (var transaction in sortTransaction)
                            {
                                Console.WriteLine(transaction.Value.Type +" - " + transaction.Value.Amount + " - " + transaction.Value.Type + " - " + transaction.Value.Category + " - "+ transaction.Value.Date);
                            }
                        }
                        
                        Console.Write("1 - Povratak");
                        var Action = false;
                        var one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }

                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                    case 2:
                        list = GiroAccTransactions[foundKey];

                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            var sortTransaction = dictionary.OrderBy(amount => amount.Value.Date).ToList();

                            foreach (var transaction in sortTransaction)
                            {
                                Console.WriteLine(transaction.Value.Type +" - " + transaction.Value.Amount + " - " + transaction.Value.Type + " - " + transaction.Value.Category + " - "+ transaction.Value.Date);
                            }
                        }
                        
                        Console.Write("1 - Povratak");
                        Action = false;
                        one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }

                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                    case 3:
                        list = PrepaidAccTransactions[foundKey];

                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            var sortTransaction = dictionary.OrderBy(amount => amount.Value.Date).ToList();

                            foreach (var transaction in sortTransaction)
                            {
                                Console.WriteLine(transaction.Value.Type +" - " + transaction.Value.Amount + " - " + transaction.Value.Type + " - " + transaction.Value.Category + " - "+ transaction.Value.Date);
                            }
                        }
                        
                        Console.Write("1 - Povratak");
                        Action = false;
                        one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }
                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                }
            }
            
            static void TransactionDateDownward(int typeOfAccount, int foundKey)
            {
                switch (typeOfAccount)
                {
                    case 1:
                        var list = CurrentAccTransactions[foundKey];

                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            var sortTransaction = dictionary.OrderByDescending(amount => amount.Value.Date).ToList();

                            foreach (var transaction in sortTransaction)
                            {
                                Console.WriteLine(transaction.Value.Type +" - " + transaction.Value.Amount + " - " + transaction.Value.Type + " - " + transaction.Value.Category + " - "+ transaction.Value.Date);
                            }
                        }
                        
                        Console.Write("1 - Povratak");
                        var Action = false;
                        var one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }
                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                    case 2:
                        list = GiroAccTransactions[foundKey];

                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            var sortTransaction = dictionary.OrderByDescending(amount => amount.Value.Date).ToList();

                            foreach (var transaction in sortTransaction)
                            {
                                Console.WriteLine(transaction.Value.Type +" - " + transaction.Value.Amount + " - " + transaction.Value.Type + " - " + transaction.Value.Category + " - "+ transaction.Value.Date);
                            }
                        }
                        
                        Console.Write("1 - Povratak");
                        Action = false;
                        one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }
                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                    case 3:
                        list = PrepaidAccTransactions[foundKey];

                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            var sortTransaction = dictionary.OrderByDescending(amount => amount.Value.Date).ToList();

                            foreach (var transaction in sortTransaction)
                            {
                                Console.WriteLine(transaction.Value.Type +" - " + transaction.Value.Amount + " - " + transaction.Value.Type + " - " + transaction.Value.Category + " - "+ transaction.Value.Date);
                            }
                        }
                        
                        Console.Write("1 - Povratak");
                        Action = false;
                        one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }

                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                }
            }
            
            static void TransactionIncome(int typeOfAccount, int foundKey)
            {
                switch (typeOfAccount)
                {
                    case 1:
                        var list = CurrentAccTransactions[foundKey];
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            foreach (var key in dictionary.Keys)
                            {
                                if (dictionary[key].Type == "prihod")
                                {
                                    Console.WriteLine(dictionary[key]);
                                }
                            }
                          
                        }
                        
                        Console.Write("1 - Povratak");
                        var Action = false;
                        var one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }
                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                    case 2:
                        list = GiroAccTransactions[foundKey];
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            foreach (var key in dictionary.Keys)
                            {
                                if (dictionary[key].Type == "prihod")
                                {
                                    Console.WriteLine(dictionary[key]);
                                }
                            }
                        }
                        Console.Write("1 - Povratak");
                        Action = false;
                        one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }
                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                    case 3:
                        list = PrepaidAccTransactions[foundKey];
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            foreach (var key in dictionary.Keys)
                            {
                                if (dictionary[key].Type == "prihod")
                                {
                                    Console.WriteLine(dictionary[key]);
                                }
                            }
                        }
                        Console.Write("1 - Povratak");
                        Action = false;
                        one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }
                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                }
            }
            
            static void TransactionExpense(int typeOfAccount, int foundKey)
            {
                switch (typeOfAccount)
                {
                    case 1:
                        
                        var list = CurrentAccTransactions[foundKey];
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            foreach (var key in dictionary.Keys)
                            {
                                if (dictionary[key].Type == "rashod")
                                {
                                    Console.WriteLine(dictionary[key]);
                                }
                            }
                        }
                        
                        Console.Write("1 - Povratak");
                        var Action = false;
                        var one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }
                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                    case 2:
                        list = CurrentAccTransactions[foundKey];
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            foreach (var key in dictionary.Keys)
                            {
                                if (dictionary[key].Type == "rashod")
                                {
                                    Console.WriteLine(dictionary[key]);
                                }
                            }
                        }
                        
                        Console.Write("1 - Povratak");
                        Action = false;
                        one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }
                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                    case 3:
                        list = CurrentAccTransactions[foundKey];
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            foreach (var key in dictionary.Keys)
                            {
                                if (dictionary[key].Type == "rashod")
                                {
                                    Console.WriteLine(dictionary[key]);
                                }
                            }
                        }
                        
                        Console.Write("1 - Povratak");
                        Action = false;
                        one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }
                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                }
            }
            
            static void TransactionCategory(int typeOfAccount, int foundKey)
            {
                switch (typeOfAccount)
                {
                    case 1:
                        var list = CurrentAccTransactions[foundKey];
                        Console.Write("Odaberite kategoriju transakcije: ");
                        var Category = "";
                        while (Category != "hrana" && Category != "prijevoz" && Category != "sport" &&
                               Category != "plaća" && Category != "honorar" && Category != "poklon")
                        {
                            Category = Console.ReadLine();
                        }
                        
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            foreach (var key in dictionary.Keys)
                            {
                                if (dictionary[key].Category == Category)
                                {
                                    Console.WriteLine(dictionary[key]);
                                }
                            }
                            
                            
                        }
                        
                        Console.Write("1 - Povratak");
                        var Action = false;
                        var one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }
                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                    case 2:
                        list = GiroAccTransactions[foundKey];
                        Console.Write("Odaberite kategoriju transakcije: ");
                        Category = "";
                        while (Category != "hrana" && Category != "prijevoz" && Category != "sport" &&
                               Category != "plaća" && Category != "honorar" && Category != "poklon")
                        {
                            Category = Console.ReadLine();
                        }
                        
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            foreach (var key in dictionary.Keys)
                            {
                                if (dictionary[key].Category == Category)
                                {
                                    Console.WriteLine(dictionary[key]);
                                }
                            }
                        }
                        
                        Console.Write("1 - Povratak");
                        Action = false;
                        one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }
                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                    case 3:
                        list = PrepaidAccTransactions[foundKey];
                        Console.Write("Odaberite kategoriju transakcije: ");
                        Category = "";
                        while (Category != "hrana" && Category != "prijevoz" && Category != "sport" &&
                               Category != "plaća" && Category != "honorar" && Category != "poklon")
                        {
                            Category = Console.ReadLine();
                        }
                        
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            foreach (var key in dictionary.Keys)
                            {
                                if (dictionary[key].Category == Category)
                                {
                                    Console.WriteLine(dictionary[key]);
                                }
                            }
                        }
                        
                        Console.Write("1 - Povratak");
                        Action = false;
                        one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }
                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                }
            }
            
            static void TransactionTypeCategory(int typeOfAccount, int foundKey)
            {
                switch (typeOfAccount)
                {
                    case 1:
                        var list = CurrentAccTransactions[foundKey];
                        Console.Write("Odaberite kategoriju transakcije: ");
                        var Category = "";
                        while (Category != "hrana" && Category != "prijevoz" && Category != "sport" &&
                               Category != "plaća" && Category != "honorar" && Category != "poklon")
                        {
                            Category = Console.ReadLine();
                        }
                        
                        Console.Write("Odaberite tip transakcije: ");
                        var Type = "";
                        while (Type != "prihdo" || Type != "rashod")
                        {
                            Type = Console.ReadLine();
                        }
                        
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            foreach (var key in dictionary.Keys)
                            {
                                if (dictionary[key].Category == Category && dictionary[key].Type == Type)
                                {
                                    Console.WriteLine(dictionary[key]);
                                }
                            }
                            
                        }
                        
                        Console.Write("1 - Povratak");
                        var Action = false;
                        var one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }
                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                    case 2:
                        list = GiroAccTransactions[foundKey];
                        Console.Write("Odaberite kategoriju transakcije: ");
                        Category = "";
                        while (Category != "hrana" && Category != "prijevoz" && Category != "sport" ||
                               Category != "plaća" && Category != "honorar" && Category != "poklon")
                        {
                            Category = Console.ReadLine();
                        }
                        
                        Console.Write("Odaberite kategoriju transakcije: ");
                        Type = "";
                        while (Type != "prihod" || Type != "rashod")
                        {
                            Type = Console.ReadLine();
                        }
                        
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            foreach (var key in dictionary.Keys)
                            {
                                if (dictionary[key].Category == Category && dictionary[key].Type == Type)
                                {
                                    Console.WriteLine(dictionary[key]);
                                }
                            }
                        }
                        
                        Console.Write("1 - Povratak");
                        Action = false;
                        one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }
                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                    case 3:
                        list = CurrentAccTransactions[foundKey];
                        Console.Write("Odaberite kategoriju transakcije: ");
                        Category = "";
                        while (Category != "hrana" && Category != "prijevoz" && Category != "sport" ||
                               Category != "plaća" && Category != "honorar" && Category != "poklon")
                        {
                            Category = Console.ReadLine();
                        }
                        
                        Console.Write("Odaberite kategoriju transakcije: ");
                        Type = "";
                        while (Type != "prihod" || Type != "rashod")
                        {
                            Type = Console.ReadLine();
                        }
                        
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i == 1)
                            {
                                break;
                            }
                            var dictionary = list[i];
                            foreach (var key in dictionary.Keys)
                            {
                                if (dictionary[key].Category == Category && dictionary[key].Type == Type)
                                {
                                    Console.WriteLine(dictionary[key]);
                                }
                            }
                        }
                        
                        Console.Write("1 - Povratak");
                        Action = false;
                        one = 0;
                        while (!Action && one != 1)
                        {
                            Action = int.TryParse(Console.ReadLine(), out one);
                        }
                        ViewInTransactions(typeOfAccount, foundKey);
                        break;
                }
            }
            
            
            static void FinancialReport(int typeOfAccount, int foundKey)
            {
                Console.WriteLine("1 - Trenutno stanje računa");
                Console.WriteLine("2 - Ukupan broj transakcija");
                Console.WriteLine("3 - ukupan iznos rashoda i prihoda za odabranu godinu");
                Console.WriteLine("4 - Postotak udjela rashoda");
                Console.WriteLine("5 - Prosječni iznos transakcije za odabrani mjesec i godinu");
                Console.WriteLine("6 - Prosječni iznos transakcije za odabranu kategoriju");
                Console.WriteLine("7 - Povratak");
                
                Console.Write("Odaberite željenu akciju: ");
                var Action = false;
                var numberOfAction = 1;
                while (!Action && numberOfAction <= 1 || numberOfAction >= 8)
                {
                    Action = int.TryParse(Console.ReadLine(), out numberOfAction);
                }
                Console.Clear();
                switch (numberOfAction)
                {
                    case 1:
                        switch (typeOfAccount)
                        {
                            case 1:
                                var list = CurrentAccTransactions[foundKey];
                                for (int i = 0; i < list.Count; i++)
                                {
                                    var dictionary = list[i];
                                    var NumberType1 = 0;
                                    var NumberType2 = 0;
                                    
                                    foreach (var key in dictionary.Keys)
                                    {
                                        if (dictionary[key].Type == "prihod")
                                        {
                                            NumberType1 += 1;
                                        }
                                        else
                                        {
                                            NumberType2 += 1;
                                        }
                                    }
                                    
                                    Console.WriteLine("Ukupan broj prihoda: " +NumberType1);
                                    Console.WriteLine("Ukupan broj rashoda: ", +NumberType2);
                                    if (UserAccounts[foundKey].CurrentAccount < 0)
                                    {
                                        Console.WriteLine("Korisnik je u minusu");
                                    }
                                    Console.Write("1 - Povratak: ");
                                    Action = false;
                                    var one = 0;
                                    while (!Action && one != 1)
                                    {
                                        Action = int.TryParse(Console.ReadLine(), out one);
                                    }
                                    Console.Clear();
                                    FinancialReport(typeOfAccount, foundKey);
                                }
                                break;
                            case 2:
                                list = GiroAccTransactions[foundKey];
                                for (int i = 0; i < list.Count; i++)
                                {
                                    var dictionary = list[i];
                                    var NumberType1 = 0;
                                    var NumberType2 = 0;
                                    
                                    foreach (var key in dictionary.Keys)
                                    {
                                        if (dictionary[key].Type == "prihod")
                                        {
                                            NumberType1 += 1;
                                        }
                                        else
                                        {
                                            NumberType2 += 1;
                                        }
                                    }

                                    Console.WriteLine("Ukupan broj prihoda: " +NumberType1);
                                    Console.WriteLine("Ukupan broj rashoda: " +NumberType2);
                                    if (UserAccounts[foundKey].GiroAccount < 0)
                                    {
                                        Console.WriteLine("Korisnik je u minusu");
                                    }

                                    Console.Write("1 - Povratak: ");
                                    Action = false;
                                    var one = 0;
                                    while (!Action && one != 1)
                                    {
                                        Action = int.TryParse(Console.ReadLine(), out one);
                                    }
                                    Console.Clear();
                                    FinancialReport(typeOfAccount, foundKey);
                                }

                                break;
                            case 3:
                                list = PrepaidAccTransactions[foundKey];
                                for (int i = 0; i < list.Count; i++)
                                {
                                    var dictionary = list[i];
                                    var NumberType1 = 0;
                                    var NumberType2 = 0;

                                    foreach (var key in dictionary.Keys)
                                    {
                                        if (dictionary[key].Type == "prihod")
                                        {
                                            NumberType1 += 1;
                                        }
                                        else
                                        {
                                            NumberType2 += 1;
                                        }
                                    }

                                    Console.WriteLine("Ukupan broj prihoda: " +NumberType1);
                                    Console.WriteLine("Ukupan broj rashoda: " +NumberType2);
                                    if (UserAccounts[foundKey].Prepaid < 0)
                                    {
                                        Console.WriteLine("Korisnik je u minusu");
                                    }

                                    Console.Write("1 - Povratak: ");
                                    Action = false;
                                    var one = 0;
                                    while (!Action && one != 1)
                                    {
                                        Action = int.TryParse(Console.ReadLine(), out one);
                                    }
                                    Console.Clear();
                                    FinancialReport(typeOfAccount, foundKey);
                                }

                                break;
                        }
                        break;
                            
                    case 2:
                        switch (typeOfAccount)
                        {
                            case 1:
                                var list = CurrentAccTransactions[foundKey];
                                var numberOfTransaction = list.Count;
                                Console.WriteLine("Ukupan broj transakcija: " +numberOfTransaction);
                                Console.Write("1 - Povratak");
                                Action = false;
                                var one = 0;
                                while (!Action && one != 1)
                                {
                                    Action = int.TryParse(Console.ReadLine(), out one);
                                }
                                Console.Clear();
                                FinancialReport(typeOfAccount, foundKey);
                                break;
                            case 2:
                                list = GiroAccTransactions[foundKey];
                                numberOfTransaction = list.Count;
                                Console.WriteLine("Ukupan broj transakcija: " +numberOfTransaction);
                                Console.Write("1 - Povratak");
                                Action = false;
                                one = 0;
                                while (!Action && one != 1)
                                {
                                    Action = int.TryParse(Console.ReadLine(), out one);
                                }
                                Console.Clear();
                                FinancialReport(typeOfAccount, foundKey);
                                break;
                            case 3:
                                list = PrepaidAccTransactions[foundKey];
                                numberOfTransaction = list.Count;
                                Console.WriteLine("Ukupan broj transakcija: " +numberOfTransaction);
                                Console.Write("1 - Povratak");
                                Action = false;
                                one = 0;
                                while (!Action && one != 1)
                                {
                                    Action = int.TryParse(Console.ReadLine(), out one);
                                }
                                Console.Clear();
                                FinancialReport(typeOfAccount, foundKey);
                                break;
                        }
                        break;
                    case 3:
                        switch (typeOfAccount)
                        {
                            case 1:
                                var list = CurrentAccTransactions[foundKey];
                                Console.WriteLine("Odaberite datum: ");
                                Action = false;
                                var Date = DateTime.Now;
                                while (!Action)
                                {
                                    Action = DateTime.TryParse(Console.ReadLine(), out Date);
                                }

                                var Number1 = 0;
                                var Number2 = 0;

                                for (int i = 0; i < list.Count; i++)
                                {
                                    var dictionary = list[i];
                                    foreach (var key in dictionary.Keys)
                                    {
                                        if (dictionary[key].Date == Date)
                                        {
                                            if (dictionary[key].Type == "prihod")
                                            {
                                                Number1 += 1;
                                            }
                                            else
                                            {
                                                Number2 += 1;
                                            }
                                        }
                                    }
                                }
                                
                                Console.WriteLine("Ukupan broj prihoda za odabrani datum: " +Number1);
                                Console.WriteLine("Ukupan broj rasdoa za odabrani datum: " +Number2);
                                Console.Write("1 - Povratak: ");
                                Action = false;
                                var one = 0;
                                while (!Action && one != 1)
                                {
                                    Action = int.TryParse(Console.ReadLine(), out one);
                                }
                                Console.Clear();
                                FinancialReport(typeOfAccount, foundKey);
                                break;
                            case 2:
                                list = GiroAccTransactions[foundKey];
                                Console.WriteLine("Odaberite datum: ");
                                Action = false;
                                Date = DateTime.Now;
                                while (!Action)
                                {
                                    Action = DateTime.TryParse(Console.ReadLine(), out Date);
                                }

                                Number1 = 0;
                                Number2 = 0;
                                
                                for (int i = 0; i < list.Count; i++)
                                {
                                    var dictionary = list[i];
                                    foreach (var key in dictionary.Keys)
                                    {
                                        if (dictionary[key].Date == Date)
                                        {
                                            if (dictionary[key].Type == "prihod")
                                            {
                                                Number1 += 1;
                                            }
                                            else
                                            {
                                                Number2 += 1;
                                            }
                                        }
                                    }
                                }
                                
                                Console.WriteLine("Ukupan broj prihoda za odabrani datum: " +Number1);
                                Console.WriteLine("Ukupan broj rasdoa za odabrani datum: " +Number2);
                                Console.Write("1 - Povratak: ");
                                Action = false;
                                one = 0;
                                while (!Action && one != 1)
                                {
                                    Action = int.TryParse(Console.ReadLine(), out one);
                                }
                                Console.Clear();
                                FinancialReport(typeOfAccount, foundKey);
                                break;
                            case 3:
                                list = PrepaidAccTransactions[foundKey];
                                Console.WriteLine("Odaberite datum: ");
                                Action = false;
                                Date = DateTime.Now;
                                while (!Action)
                                {
                                    Action = DateTime.TryParse(Console.ReadLine(), out Date);
                                }

                                Number1 = 0;
                                Number2 = 0;
                                
                                for (int i = 0; i < list.Count; i++)
                                {
                                    var dictionary = list[i];

                                    foreach (var key in dictionary.Keys)
                                    {
                                        if (dictionary[key].Date == Date)
                                        {
                                            if (dictionary[key].Type == "prihod")
                                            {
                                                Number1 += 1;
                                            }
                                            else
                                            {
                                                Number2 += 1;
                                            }
                                        }
                                    }
                                    
                                }
                                
                                Console.WriteLine("Ukupan broj prihoda za odabrani datum: " +Number1);
                                Console.WriteLine("Ukupan broj rasdoa za odabrani datum: " +Number2);
                                Console.Write("1 - Povratak: ");
                                Action = false;
                                one = 0;
                                while (!Action && one != 1)
                                {
                                    Action = int.TryParse(Console.ReadLine(), out one);
                                }
                                Console.Clear();
                                FinancialReport(typeOfAccount, foundKey);
                                break;
                        }
                        break;
                    case 4:
                        switch (typeOfAccount)
                        {
                            case 1:
                                var list = CurrentAccTransactions[foundKey];
                                var Sport = 0;
                                var Food = 0;
                                var Bus = 0;
                                var All = 0;
                                for (int i = 0; i < list.Count; i++)
                                {
                                    var dictionary = list[i];

                                    foreach (var key in dictionary.Keys)
                                    {
                                        if (dictionary[key].Type == "rashod")
                                        {
                                            var Category = dictionary[key].Category;

                                            switch (Category)
                                            {
                                                case "sport":
                                                    Sport += 1;
                                                    All += 1;
                                                    break;
                                                case "hrana":
                                                    Food += 1;
                                                    All += 1;
                                                    break;
                                                case "prijevoz":
                                                    Bus += 1;
                                                    All += 1;
                                                    break;
                                            }
                                        }
                                    }
                                    
                                }

                                if (All == 0)
                                {
                                    Console.WriteLine("Nema rashoda");
                                    Console.WriteLine("1 - Povratak: ");
                                    Action = false;
                                    var number = 0;
                                    while (!Action && number != 1)
                                    {
                                        Action = int.TryParse(Console.ReadLine(), out number);
                                    }
                                    Console.Clear();
                                    FinancialReport(typeOfAccount, foundKey);
                                }

                                double PercenteSport = (All / Sport) * 100;
                                double PercenteFood = (All / Food) * 100;
                                double PercenteBus = (All / Bus) * 100;
                                
                                Console.WriteLine("Postotak troškova na sport: " +PercenteSport);
                                Console.WriteLine("Postotak troškova na hranu: " +PercenteFood);
                                Console.WriteLine("Postotak troškova na prijevoz: " +PercenteBus);
                                
                                Console.WriteLine("1 - Povratak");
                                Action = false;
                                var one = 0;
                                while (!Action && one != 1)
                                {
                                    Action = int.TryParse(Console.ReadLine(), out one);
                                }
                                Console.Clear();
                                FinancialReport(typeOfAccount, foundKey);
                                break;
                            case 2:
                                list = GiroAccTransactions[foundKey];
                                Sport = 0;
                                Food = 0;
                                Bus = 0;
                                All = 0;
                                for (int i = 0; i < list.Count; i++)
                                {
                                    var dictionary = list[i];

                                    foreach (var key in dictionary.Keys)
                                    {
                                        if (dictionary[key].Type == "rashod")
                                        {
                                            var Category = dictionary[key].Category;

                                            switch (Category)
                                            {
                                                case "sport":
                                                    Sport += 1;
                                                    All += 1;
                                                    break;
                                                case "hrana":
                                                    Food += 1;
                                                    All += 1;
                                                    break;
                                                case "prijevoz":
                                                    Bus += 1;
                                                    All += 1;
                                                    break;
                                            }
                                        }
                                    }
                                    
                                }
                                
                                if (All == 0)
                                {
                                    Console.WriteLine("Nema rashoda");
                                    Console.WriteLine("1 - Povratak: ");
                                    Action = false;
                                    var number = 0;
                                    while (!Action && number != 1)
                                    {
                                        Action = int.TryParse(Console.ReadLine(), out number);
                                    }
                                    Console.Clear();
                                    FinancialReport(typeOfAccount, foundKey);
                                }
                                
                                PercenteSport = (All / Sport) * 100;
                                PercenteFood = (All / Food) * 100;
                                PercenteBus = (All / Bus) * 100;
                                
                                Console.WriteLine("Postotak troškova na sport: " +PercenteSport);
                                Console.WriteLine("Postotak troškova na hranu: " +PercenteFood);
                                Console.WriteLine("Postotak troškova na prijevoz: " +PercenteBus);
                                
                                Console.WriteLine("1 - Povratak");
                                Action = false;
                                one = 0;
                                while (!Action && one != 1)
                                {
                                    Action = int.TryParse(Console.ReadLine(), out one);
                                }
                                Console.Clear();
                                FinancialReport(typeOfAccount, foundKey);
                                break;
                            case 3:
                                list = PrepaidAccTransactions[foundKey];
                                Sport = 0;
                                Food = 0;
                                Bus = 0;
                                All = 0;
                                for (int i = 0; i < list.Count; i++)
                                {
                                    var dictionary = list[i];

                                    foreach (var key in dictionary.Keys)
                                    {
                                        if (dictionary[key].Type == "rashod")
                                        {
                                            var Category = dictionary[key].Category;

                                            switch (Category)
                                            {
                                                case "sport":
                                                    Sport += 1;
                                                    All += 1;
                                                    break;
                                                case "hrana":
                                                    Food += 1;
                                                    All += 1;
                                                    break;
                                                case "prijevoz":
                                                    Bus += 1;
                                                    All += 1;
                                                    break;
                                            }
                                        }
                                    }
                                    
                                }
                                
                                if (All == 0)
                                {
                                    Console.WriteLine("Nema rashoda");
                                    Console.WriteLine("1 - Povratak: ");
                                    Action = false;
                                    var number = 0;
                                    while (!Action && number != 1)
                                    {
                                        Action = int.TryParse(Console.ReadLine(), out number);
                                    }
                                    Console.Clear();
                                    FinancialReport(typeOfAccount, foundKey);
                                }
                                
                                PercenteSport = (All / Sport) * 100;
                                PercenteFood = (All / Food) * 100;
                                PercenteBus = (All / Bus) * 100;
                                
                                Console.WriteLine("Postotak troškova na sport: " +PercenteSport);
                                Console.WriteLine("Postotak troškova na hranu: " +PercenteFood);
                                Console.WriteLine("Postotak troškova na prijevoz: " +PercenteBus);
                                
                                Console.WriteLine("1 - Povratak");
                                Action = false;
                                one = 0;
                                while (!Action && one != 1)
                                {
                                    Action = int.TryParse(Console.ReadLine(), out one);
                                }
                                
                                Console.Clear();
                                FinancialReport(typeOfAccount, foundKey);
                                break;
                        }
                        break;
                    case 5:
                        switch (typeOfAccount)
                        {
                            case 1:
                                var list = CurrentAccTransactions[foundKey];
                                Console.WriteLine("Odaberite datum: ");
                                Action = false;
                                var Date = DateTime.Now;
                                while (!Action)
                                {
                                    Action = DateTime.TryParse(Console.ReadLine(), out Date);
                                }

                                var DateYear = Date.Year;
                                var DateMonth = Date.Month;
                                double Number = 0;
                                double NumberAmount = 0;
                                
                                for (int i = 0; i < list.Count; i++)
                                {
                                    var dictionary = list[i];
                                    foreach (var key in dictionary.Keys)
                                    {
                                        var DateTransaction = dictionary[key].Date;
                                        var DateTransactionYear = DateTransaction.Year;
                                        var DateTransactionMonth = DateTransaction.Month;

                                        if (DateTransactionYear == DateYear && DateTransactionMonth == DateMonth)
                                        {
                                            NumberAmount += dictionary[key].Amount;
                                            Number += 1;
                                        }
                                    }
                                    
                                }
                                
                                var Average = NumberAmount / Number;
                                Console.WriteLine("Prosječni iznos transakcije je: " +Average);
                                
                                Action = false;
                                var one = 0;
                                while (!Action && one != 1)
                                {
                                    Action = int.TryParse(Console.ReadLine(), out one);
                                }
                                Console.Clear();
                                FinancialReport(typeOfAccount, foundKey);
                                break;
                            case 2:
                                list = GiroAccTransactions[foundKey];
                                Console.WriteLine("Odaberite datum: ");
                                Action = false;
                                Date = DateTime.Now;
                                while (!Action)
                                {
                                    Action = DateTime.TryParse(Console.ReadLine(), out Date);
                                }

                                DateYear = Date.Year;
                                DateMonth = Date.Month;
                                Number = 0;
                                NumberAmount = 0;
                                
                                for (int i = 0; i < list.Count; i++)
                                {
                                    var dictionary = list[i];
                                    foreach (var key in dictionary.Keys)
                                    {
                                        var DateTransaction = dictionary[key].Date;
                                        var DateTransactionYear = DateTransaction.Year;
                                        var DateTransactionMonth = DateTransaction.Month;

                                        if (DateTransactionYear == DateYear && DateTransactionMonth == DateMonth)
                                        {
                                            NumberAmount += dictionary[key].Amount;
                                            Number += 1;
                                        }
                                    }
                                    
                                }
                                
                                Average = NumberAmount / Number;
                                Console.WriteLine("Prosječni iznos transakcije je: " +Average);
                                
                                Action = false;
                                one = 0;
                                while (!Action && one != 1)
                                {
                                    Action = int.TryParse(Console.ReadLine(), out one);
                                }
                                Console.Clear();
                                FinancialReport(typeOfAccount, foundKey);
                                break;
                            case 3:
                                list = PrepaidAccTransactions[foundKey];
                                Console.WriteLine("Odaberite datum: ");
                                Action = false;
                                Date = DateTime.Now;
                                while (!Action)
                                {
                                    Action = DateTime.TryParse(Console.ReadLine(), out Date);
                                }

                                DateYear = Date.Year;
                                DateMonth = Date.Month;
                                Number = 0;
                                NumberAmount = 0;
                                
                                for (int i = 0; i < list.Count; i++)
                                {
                                    var dictionary = list[i];
                                    foreach (var key in dictionary.Keys)
                                    {
                                        var DateTransaction = dictionary[key].Date;
                                        var DateTransactionYear = DateTransaction.Year;
                                        var DateTransactionMonth = DateTransaction.Month;

                                        if (DateTransactionYear == DateYear && DateTransactionMonth == DateMonth)
                                        {
                                            NumberAmount += dictionary[key].Amount;
                                            Number += 1;
                                        }
                                    }
                                    
                                }
                                
                                Average = NumberAmount / Number;
                                Console.WriteLine("Prosječni iznos transakcije je: " +Average);
                                
                                Action = false;
                                one = 0;
                                while (!Action && one != 1)
                                {
                                    Action = int.TryParse(Console.ReadLine(), out one);
                                }
                                Console.Clear();
                                FinancialReport(typeOfAccount, foundKey);
                                break;
                        }
                        break;
                    case 6:
                        switch (typeOfAccount)
                        { 
                            case 1:
                                var list = CurrentAccTransactions[foundKey];
                                Console.WriteLine("Odaberite kategoriju: ");
                                var Category = "";
                                while(Category != "plaća" && Category != "honorar" && Category != "poklon" && Category != "sport" && Category != "prijevoz" && Category != "hrana")
                                {
                                    Category = Console.ReadLine();
                                }

                                var Number = 0;
                                var Average = 0;
                                var NumberAmount = 0;
                                
                                for (int i = 0; i < list.Count; i++)
                                {
                                    var dictionary = list[i];
                                    foreach (var key in dictionary.Keys)
                                    {
                                        var CategoryTransaction = dictionary[key].Category;

                                        if (CategoryTransaction == Category)
                                        {
                                            NumberAmount += 1;
                                        }

                                        Number += 1;
                                    }
                                    
                                }

                                if (Number == 0)
                                {
                                    Console.WriteLine("Niti jedna transakcija ne sadržava odabranu kategoriju");
                                    Console.Write("1 - Povratak: ");
                                    Action = false;
                                    var number = 0;
                                    while (!Action && number != 1)
                                    {
                                        Action = int.TryParse(Console.ReadLine(), out number);
                                    }
                                    Console.Clear();
                                    FinancialReport(typeOfAccount, foundKey);
                                }
                                
                                Average = NumberAmount / Number;
                                Console.WriteLine("Prosječni iznos transakcije za odabranu kategoriju je: " +Average);
                                Console.Write("1 - Povratak: ");
                                Action = false;
                                var one = 0;
                                while (!Action && one != 1)
                                {
                                    Action = int.TryParse(Console.ReadLine(), out one);
                                }
                                Console.Clear();
                                FinancialReport(typeOfAccount, foundKey);
                                
                                break;
                            case 2:
                                list = GiroAccTransactions[foundKey];
                                Console.WriteLine("Odaberite kategoriju: ");
                                Category = "";
                                while(Category == "plaća" && Category != "honorar" && Category != "poklon" && Category != "sport" && Category != "prijevoz" && Category != "hrana")
                                {
                                    Category = Console.ReadLine();
                                }

                                Number = 0;
                                Average = 0;
                                NumberAmount = 0;
                                
                                for (int i = 0; i < list.Count; i++)
                                {
                                    var dictionary = list[i];
                                    foreach (var key in dictionary.Keys)
                                    {
                                        var CategoryTransaction = dictionary[key].Category;

                                        if (CategoryTransaction == Category)
                                        {
                                            NumberAmount += 1;
                                        }

                                        Number += 1;
                                    }
                                    
                                }

                                if (Number == 0)
                                {
                                    Console.WriteLine("Niti jedna transakcija ne sadržava odabranu kategoriju");
                                    Console.Write("1 - Povratak: ");
                                    Action = false;
                                    var number = 0;
                                    while (!Action && number != 1)
                                    {
                                        Action = int.TryParse(Console.ReadLine(), out number);
                                    }
                                    Console.Clear();
                                    FinancialReport(typeOfAccount, foundKey);
                                }
                                
                                Average = NumberAmount / Number;
                                Console.WriteLine("Prosječni iznos transakcije za odabranu kategoriju je: " +Average);
                                Console.Write("1 - Povratak: ");
                                Action = false;
                                one = 0;
                                while (!Action && one != 1)
                                {
                                    Action = int.TryParse(Console.ReadLine(), out one);
                                }
                                Console.Clear();
                                FinancialReport(typeOfAccount, foundKey);
                                
                                break;
                            case 3:
                                list = PrepaidAccTransactions[foundKey];
                                Console.WriteLine("Odaberite kategoriju: ");
                                Category = "";
                                while(Category == "plaća" && Category != "honorar" && Category != "poklon" && Category != "sport" && Category != "prijevoz" && Category != "hrana")
                                {
                                    Category = Console.ReadLine();
                                }

                                Number = 0;
                                Average = 0;
                                NumberAmount = 0;
                                
                                for (int i = 0; i < list.Count; i++)
                                {
                                    var dictionary = list[i];
                                    foreach (var key in dictionary.Keys)
                                    {
                                        var CategoryTransaction = dictionary[key].Category;

                                        if (CategoryTransaction == Category)
                                        {
                                            NumberAmount += 1;
                                        }

                                        Number += 1;
                                    }
                                    
                                }

                                if (Number == 0)
                                {
                                    Console.WriteLine("Niti jedna transakcija ne sadržava odabranu kategoriju");
                                    Console.Write("1 - Povratak: ");
                                    Action = false;
                                    var number = 0;
                                    while (!Action && number != 1)
                                    {
                                        Action = int.TryParse(Console.ReadLine(), out number);
                                    }
                                    Console.Clear();
                                    FinancialReport(typeOfAccount, foundKey);
                                }
                                
                                Average = NumberAmount / Number;
                                Console.WriteLine("Prosječni iznos transakcije za odabranu kategoriju je: " +Average);
                                Console.Write("1 - Povratak: ");
                                Action = false;
                                one = 0;
                                while (!Action && one != 1)
                                {
                                    Action = int.TryParse(Console.ReadLine(), out one);
                                }
                                Console.Clear();
                                FinancialReport(typeOfAccount, foundKey);
                                
                                break;
                        }
                        break;
                    case 7:
                        Console.Clear();
                        TransactionManagment(typeOfAccount, foundKey);
                        break;
                }


            }
            
        }
    }

    
}

 

       