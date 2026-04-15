using System;

class Product
{
    public int Id;
    public string Name;
    public double Price;
    public int RemainingStock;

    public Product(int id, string name, double price, int stock)
    {
        Id = id;
        Name = name;
        Price = price;
        RemainingStock = stock;
    }

    public void DisplayProduct()
    {
        Console.WriteLine($"{Id}. {Name} - ₱{Price} (Stock: {RemainingStock})");
    }

    public double GetItemTotal(int quantity)
    {
        return Price * quantity;
    }

    public bool HasEnoughStock(int quantity)
    {
        return quantity <= RemainingStock;
    }

    public void DeductStock(int quantity)
    {
        RemainingStock -= quantity;
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        Product[] products = new Product[]
        {
            new Product(1, "Laptop", 54381, 5),
            new Product(2, "Mouse", 4917, 10),
            new Product(3, "Keyboard", 2699, 7),
            new Product(4, "Headphones", 3600, 3),
            new Product(5, "Monitor", 5000, 7),
            new Product(6, "Microphone", 8499, 7),
            new Product(7, "CPU", 16899, 9),
            new Product(8, "GPU", 33999, 6)

        };

        int[] cartIds = new int[10];
        int[] cartQty = new int[10];
        double[] cartSubtotal = new double[10];
        int cartCount = 0;

        string choice = "Y"; //

        do
        {
            Console.WriteLine("\n             MENU        ");
            for (int i = 0; i < products.Length; i++)
            {
                products[i].DisplayProduct();
            }

            Console.Write("Enter product number: ");
            if (!int.TryParse(Console.ReadLine(), out int pNum))
            {
                Console.WriteLine("Invalid input.");
                continue;
            }

            if (pNum < 1 || pNum > products.Length)
            {
                Console.WriteLine("Invalid product number.");
                continue;
            }

            Product selected = products[pNum - 1];

            if (selected.RemainingStock == 0)
            {
                Console.WriteLine("Out of stock.");
                continue;
            }

            Console.Write("Enter quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0)
            {
                Console.WriteLine("Invalid quantity.");
                continue;
            }

            if (!selected.HasEnoughStock(qty))
            {
                Console.WriteLine("Not enough stock available.");
                continue;
            }

            double total = selected.GetItemTotal(qty);

            int foundIndex = -1;
            for (int i = 0; i < cartCount; i++)
            {
                if (cartIds[i] == selected.Id)
                {
                    foundIndex = i;
                    break;
                }
            }

            if (foundIndex != -1)
            {
                cartQty[foundIndex] += qty;
                cartSubtotal[foundIndex] += total;
            }
            else
            {
                if (cartCount == cartIds.Length)
                {
                    Console.WriteLine("Cart is full.");
                    continue;
                }

                cartIds[cartCount] = selected.Id;
                cartQty[cartCount] = qty;
                cartSubtotal[cartCount] = total;
                cartCount++;
            }

            selected.DeductStock(qty);

            Console.WriteLine("Added to cart!");

            
            Console.Write("Add another item? (Y/N): ");
            string input = Console.ReadLine();

            if (input == null)
                choice = "N";
            else
                choice = input.Trim().ToUpper();

        } while (choice == "Y");

        Console.WriteLine("\n            RECEIPT         ");
        double grandTotal = 0;

        for (int i = 0; i < cartCount; i++)
        {
            Product p = null;

            for (int j = 0; j < products.Length; j++)
            {
                if (products[j].Id == cartIds[i])
                {
                    p = products[j];
                    break;
                }
            }

            Console.WriteLine($"{p.Name} - Qty: {cartQty[i]} - ₱{cartSubtotal[i]}");
            grandTotal += cartSubtotal[i];
        }

        Console.WriteLine($"\nGrand Total: ₱{grandTotal}");

        double discount = 0;
        if (grandTotal >= 5000)
        {
            discount = grandTotal * 0.10;
            Console.WriteLine($"Discount (10%): -₱{discount}");
        }

        double finalTotal = grandTotal - discount;
        Console.WriteLine($"Final Total: ₱{finalTotal}");

        Console.WriteLine("\n           UPDATED STOCK        ");
        for (int i = 0; i < products.Length; i++)
        {
            products[i].DisplayProduct();
        }
    }
}
