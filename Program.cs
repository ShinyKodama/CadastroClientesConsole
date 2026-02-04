using System;
using System.Text.Json;
using System.Text.Json.Serialization;

//☰ █
class Program
{
    static List<Cliente> clientes = new List<Cliente>();    
    static void Main(String[] args)
    {
        WaitInit();
        while (true)
        {
            Menu();
            string opcao; int op;
            do
            {
                Console.Clear();
                Menu();
                Console.Write(" Digite o número da opção desejada: ");
                opcao = Console.ReadLine();
            } while (!int.TryParse(opcao, out op) || op < 0 || string.IsNullOrWhiteSpace(opcao));

            if (op == 0)
            {
                for (int i = 3; i >= 1; i--)
                {
                    Console.Clear();
                    Console.WriteLine($" Saindo em...{i}");
                    Thread.Sleep(800);
                }
                return;
            } 
            else if (op == 1)
            {
                WaitInit();
                NovoCadastro();
            }
            else if (op == 2)
            {
                WaitInit();
                ListarCadastros();
            }
            else if (op == 3)
            {
                if (clientes.Count == 0)
                {
                    Console.Clear();
                    WaitInit();
                    
                    Console.WriteLine($"   {new string('-', 43)}");
                    
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("        NENHUM REGISTRO FOI FEITO AINDA! ");
                    Console.ResetColor();
                    
                    Console.WriteLine($"   {new string('-', 43)}");
                    VoltarAoMenu();  
                }
                else
                {
                    WaitInit();
                    Console.Clear();
                    
                    Console.Write(" Cadastro salvo com ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(" sucesso!");
                    Console.ResetColor();
                    
                    SalvarCadastro(@"C:\Users\Lucas\Desktop\Dados");    
                }
            }
            else if (op == 4)
            {
                string titulo = "CARREGAR CADASTROS JSON";
                Console.Clear();
                
                WaitInit();
                Console.WriteLine($"{new string('-', 23)} {titulo} {new string('-', 23)}");
                Console.WriteLine(" Caminho disponível: ");
                
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("=> C:/Users/Lucas/Desktop/Dados");
                Console.ResetColor();                
                
                Console.Write(" Aperte qualquer tecla para carregar os dados...");
                Console.ReadKey();
                Console.Clear();
                CarregarCadastro(@"C:/Users/Lucas/Desktop/Dados");
            }
        }
    }


    static void NovoCadastro()
    {
        string titulo = "REALIZAR NOVO CADASTRO", nome = "", IDADE = "", CPF = "";
        int idade = 0; long cpf = 0;
        
        do
        {
            Console.Clear();
            Console.WriteLine($" {new string('-', 10)} {titulo} {new string('-', 10)}");
            
            RequisitarDadoCliente("Nome",  ConsoleColor.Gray, ConsoleColor.DarkCyan);
            nome = Console.ReadLine() ?? "";
            
            if (string.IsNullOrWhiteSpace(nome) || !nome.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
                continue;

            RequisitarDadoCliente("Idade", ConsoleColor.Gray, ConsoleColor.DarkCyan);
            IDADE = Console.ReadLine() ?? "";
            

            if (!int.TryParse(IDADE, out idade) || idade <= 0 || idade > 128)
            {
                while(true)
                {
                    Console.Clear();
                    Console.WriteLine($" {new string('-', 10)} {titulo} {new string('-', 10)}");
                    
                    RequisitarDadoCliente("Nome",  ConsoleColor.Gray, ConsoleColor.DarkCyan);
                    Console.WriteLine(nome);

                    RequisitarDadoCliente("Idade", ConsoleColor.Gray, ConsoleColor.DarkCyan);
                    IDADE = Console.ReadLine();

                    if (!IDADE.All(i => char.IsDigit(i) || char.IsWhiteSpace(i)))
                        continue;
                    
                    if (int.TryParse(IDADE, out idade) && idade > 0 && idade < 128)
                        break;     
                }   
            }

            RequisitarDadoCliente("CPF",   ConsoleColor.Gray, ConsoleColor.DarkCyan);
            CPF = (Console.ReadLine() ?? "").Trim();    
            
            CPF = CPF.Trim();
            
            if (CPF.Length != 11)
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine($" {new string('-', 10)} {titulo} {new string('-', 10)}");
                    RequisitarDadoCliente("Nome",  ConsoleColor.Gray, ConsoleColor.DarkCyan);
                    Console.WriteLine(nome);
                    
                    RequisitarDadoCliente("Idade", ConsoleColor.Gray, ConsoleColor.DarkCyan);
                    Console.WriteLine(IDADE);
                    
                    RequisitarDadoCliente("CPF",   ConsoleColor.Gray, ConsoleColor.DarkCyan);
                    CPF = (Console.ReadLine() ?? "").Trim();
                    
                    if (CPF.Length == 11) 
                        break;  
                }
            }

        } while (string.IsNullOrWhiteSpace(IDADE)  
                || string.IsNullOrWhiteSpace(CPF) || !long.TryParse(CPF, out cpf)
                || !int.TryParse(IDADE, out idade));

        Cliente c = new Cliente(nome, idade, cpf);
        clientes.Add(c);

        Console.Write(" Cadastro realizado com");
        Console.ForegroundColor = ConsoleColor.Green; 
        Console.WriteLine(" sucesso!"); 
        Console.ResetColor();
        VoltarAoMenu();
    
    }
    static void ListarCadastros()
    {
        string titulo = "LISTA DE CADASTROS REALIZADOS";
        Console.Clear();
        Console.WriteLine($"{new string('-', 10)} {titulo} {new string('-', 10)}");
        if (clientes.Count == 0)
        {
            Console.WriteLine($"   {new string('-', 43)}");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("        NENHUM REGISTRO FOI FEITO AINDA! ");
            Console.ResetColor();
            Console.WriteLine($"   {new string('-', 43)}");
            VoltarAoMenu();  
        } 
        else
        {
            foreach (dynamic c in clientes)
            {
                Console.WriteLine($"{new string('-', 43)}");
                c.ListarCliente();
                Console.WriteLine($"{new string('-', 43)}");
            }
            Console.WriteLine($"{new string('-', titulo.Length + 23)}");
            VoltarAoMenu();    
        }
    }

    static void SalvarCadastro(string pasta)
    {
        Directory.CreateDirectory(pasta);
        string file = Path.Combine(pasta, "clientes.json");
        var json = JsonSerializer.Serialize(clientes, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(file, json);
        VoltarAoMenu();
    }
    static void CarregarCadastro(string PATH)
    {
        string file = Path.Combine(PATH, "clientes.json");
        
        if (!File.Exists(file))
        {
            ErrorData("(X) ARQUIVO NÃO ENCONTRADO!");
            return;
        }
            
        var json = File.ReadAllText(file);
        var lista = JsonSerializer.Deserialize<List<Cliente>>(json) ?? new List<Cliente>();

        clientes = lista;

         if (clientes.Count > 0)
            typeof(Cliente).GetField("nextid", System.Reflection.BindingFlags.Static | 
            System.Reflection.BindingFlags.NonPublic) ?.SetValue(null, clientes.Max(c => c.Id) + 1);
        
        VoltarAoMenu();
    }

    static void ErrorData(string texto)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(texto);
        Console.ResetColor();
    }
    static void RequisitarDadoCliente(string dado, ConsoleColor corTexto, ConsoleColor corDado)
    {
        Console.ForegroundColor = corTexto;
        Console.Write(" Insira o ");

        Console.ForegroundColor = corDado;
        Console.Write($"{dado} ");

        Console.ResetColor();
        Console.Write("do(a) cliente: ");
    }
    static void VoltarAoMenu()
    {
        Thread.Sleep(500);
        Console.Write("\n Aperte");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(" qualquer tecla");
        Console.ResetColor();
        Console.Write(" para voltar ao menu...");
        Console.ReadKey(true);
    }
    static void Menu()
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.Gray;
        string titulo = "SEJA BEM-VINDO(A) AO SEU MENU DE CADASTRO!";
        
        Console.WriteLine($" {new string('-', 10)} {titulo} {new string('-', 10)} ");        
        OpcaoMenu(1, " Novo Cadastro...",       ConsoleColor.DarkCyan, ConsoleColor.Yellow);
        OpcaoMenu(2, " Listar Cadastros...",    ConsoleColor.DarkCyan, ConsoleColor.Yellow);
        OpcaoMenu(3, " Salvar Cadastros...",     ConsoleColor.DarkCyan, ConsoleColor.Yellow);
        OpcaoMenu(4, " Carregar Cadastros... ",  ConsoleColor.DarkCyan, ConsoleColor.Yellow);
        OpcaoMenu(5, " Apagar Cadastro... ",    ConsoleColor.DarkCyan, ConsoleColor.Yellow);
        OpcaoMenu(0, " Sair do programa... ",   ConsoleColor.Red,      ConsoleColor.Red);
        
        Console.WriteLine($"{new string('-', titulo.Length + 23)}");
        Console.ResetColor();
    }
    static void WaitInit()
    {
        for (int i = 3; i >= 1; i--)
        {
            Console.Clear();
            Console.WriteLine($" Iniciando em... {i} \n");

            int total = 30;
            int preenchido = (int)((3 - i + 1) / 3.0 * total);

            Console.WriteLine(
                new string('█', preenchido) +
                new string('☰', total - preenchido)
            );

            Thread.Sleep(700);
        }

        Console.Clear();
        Console.WriteLine(" Iniciando agora! \n");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(new string('█', 30));
        Thread.Sleep(800);
        Console.ResetColor();
    }
    static void OpcaoMenu(int num, string texto, ConsoleColor corNum, ConsoleColor corText)
    {
        Console.ForegroundColor = corNum;
        Console.Write($" {num:00}");
    
        Console.ForegroundColor = corText;
        Console.WriteLine($" -> {texto}");

        Console.ResetColor();
    }
}
class Cliente
{
    static int nextid = 1;
    public int Id       { get; set; }
    public string Nome  { get; set; } = "";
    public int Idade    { get; set; }
    public long CPF     { get; set; }


    public Cliente() {}
    public Cliente(string n, int i, long cpf)
    {
        Id = nextid++;
        Nome = n;
        Idade = i;
        CPF = cpf;        
    }
    public void ListarCliente()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(" - ID: ");
        Console.ResetColor();
        Console.WriteLine(Id);

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(" - Nome: ");
        Console.ResetColor();
        Console.WriteLine(Nome);

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(" - Idade: ");
        Console.ResetColor();
        Console.WriteLine(Idade);

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(" - CPF: ");
        Console.ResetColor();
        Console.WriteLine(CPF);
    } 

}