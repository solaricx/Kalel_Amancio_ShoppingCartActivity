using System;

class Product
{
    public int Id;
    public string Name;
    public double Price;
    public int RemainingStock;
    public string Category;

    public Product(int id, string name, double price, int stock, string category)
    {
        Id = id;
        Name = name;
        Price = price;
        RemainingStock = stock;
        Category = category;
    }

    public void DisplayProduct()
    {
        Console.WriteLine($"{Id}. {Name} - ₱{Price} (Stock: {RemainingStock}) [{Category}]");
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
            new Product(1, "Laptop", 54381, 5, "Electronics"),
            new Product(2, "Mouse", 4917, 10, "Electronics"),
            new Product(3, "Keyboard", 2699, 7, "Electronics"),
            new Product(4, "Headphones", 3600, 3, "Electronics"),
            new Product(5, "Monitor", 5000, 7, "Electronics"),
            new Product(6, "Microphone", 8499, 7, "Electronics"),
            new Product(7, "CPU", 16899, 9, "Electronics"),
            new Product(8, "GPU", 33999, 6, "Electronics")
        };

        int[] cartIds = new int[10];
        int[] cartQty = new int[10];
        double[] cartSubtotal = new double[10];
        int cartCount = 0;

        string[] orderHistory = new string[10];
        int orderCount = 0;

        bool running = true;

        while (running)
        {
            Console.WriteLine("\n   MAIN MENU   ");
            Console.WriteLine("1. Add to Cart");
            Console.WriteLine("2. View Cart");
            Console.WriteLine("3. Remove Item");
            Console.WriteLine("4. Update Quantity");
            Console.WriteLine("5. Clear Cart");
            Console.WriteLine("6. Search Product");
            Console.WriteLine("7. Filter Category");
            Console.WriteLine("8. Checkout");
            Console.WriteLine("9. Exit");

            Console.Write("Choice: ");
            if (!int.TryParse(Console.ReadLine(), out int choice)) continue;

            switch (choice)
            {
                case 1:
                    DisplayProducts(products);
                    AddToCart(products, cartIds, cartQty, cartSubtotal, ref cartCount);
                    break;
                case 2:
                    ViewCart(products, cartIds, cartQty, cartSubtotal, cartCount);
                    break;
                case 3:
                    RemoveItem(cartIds, cartQty, cartSubtotal, ref cartCount);
                    break;
                case 4:
                    UpdateQuantity(cartIds, cartQty, cartSubtotal, products, cartCount);
                    break;
                case 5:
                    cartCount = 0;
                    Console.WriteLine("Cart cleared.");
                    break;
                case 6:
                    SearchProduct(products);
                    break;
                case 7:
                    FilterCategory(products);
                    break;
                case 8:
                    Checkout(products, cartIds, cartQty, cartSubtotal, cartCount, orderHistory, ref orderCount);
                    cartCount = 0;
                    break;
                case 9:
                    running = false;
                    break;
            }
        }
    }

    static void DisplayProducts(Product[] products)
    {
        Console.WriteLine("n    PRODUCTS    ");
        foreach (var p in products)
        {
            p.DisplayProduct();
        }
    }

    static void AddToCart(Product[] products, int[] cartIds, int[] cartQty, double[] cartSubtotal, ref int cartCount)
    {
        Console.Write("Enter product ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;

        Product selected = null;
        foreach (var p in products)
        {
            if (p.Id == id)
                selected = p;
        }

        if (selected == null) return;

        Console.Write("Enter quantity: ");
        if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0) return;

        if (!selected.HasEnoughStock(qty))
        {
            Console.WriteLine("Not enough stock.");
            return;
        }

        double total = selected.GetItemTotal(qty);

        int found = -1;
        for (int i = 0; i < cartCount; i++)
        {
            if (cartIds[i] == selected.Id)
                found = i;
        }

        if (found != -1)
        {
            cartQty[found] += qty;
            cartSubtotal[found] += total;
        }
        else
        {
            cartIds[cartCount] = selected.Id;
            cartQty[cartCount] = qty;
            cartSubtotal[cartCount] = total;
            cartCount++;
        }

        selected.DeductStock(qty);
        Console.WriteLine("Added to cart!");
    }

    static void ViewCart(Product[] products, int[] cartIds, int[] cartQty, double[] cartSubtotal, int cartCount)
    {
        Console.WriteLine("\n   CART    ");
        double total = 0;

        for (int i = 0; i < cartCount; i++)
        {
            foreach (var p in products)
            {
                if (p.Id == cartIds[i])
                {
                    Console.WriteLine($"{p.Name} x{cartQty[i]} = ₱{cartSubtotal[i]}");
                    total += cartSubtotal[i];
                }
            }
        }

        Console.WriteLine($"Total: ₱{total}");
    }

    static void RemoveItem(int[] cartIds, int[] cartQty, double[] cartSubtotal, ref int cartCount)
    {
        Console.Write("Enter product ID to remove: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;

        for (int i = 0; i < cartCount; i++)
        {
            if (cartIds[i] == id)
            {
                for (int j = i; j < cartCount - 1; j++)
                {
                    cartIds[j] = cartIds[j + 1];
                    cartQty[j] = cartQty[j + 1];
                    cartSubtotal[j] = cartSubtotal[j + 1];
                }
                cartCount--;
                Console.WriteLine("Item removed.");
                return;
            }
        }
    }

    static void UpdateQuantity(int[] cartIds, int[] cartQty, double[] cartSubtotal, Product[] products, int cartCount)
    {
        Console.Write("Enter product ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;

        for (int i = 0; i < cartCount; i++)
        {
            if (cartIds[i] == id)
            {
                Console.Write("New quantity: ");
                if (!int.TryParse(Console.ReadLine(), out int qty)) return;

                foreach (var p in products)
                {
                    if (p.Id == id)
                    {
                        cartQty[i] = qty;
                        cartSubtotal[i] = p.Price * qty;
                    }
                }
            }
        }
    }

    static void SearchProduct(Product[] products)
    {
        Console.Write("Search: ");
        string keyword = Console.ReadLine().ToLower();

        foreach (var p in products)
        {
            if (p.Name.ToLower().Contains(keyword))
                p.DisplayProduct();
        }
    }

    static void FilterCategory(Product[] products)
    {
        Console.Write("Enter category: ");
        string cat = Console.ReadLine();

        foreach (var p in products)
        {
            if (p.Category.Equals(cat, StringComparison.OrdinalIgnoreCase))
                p.DisplayProduct();
        }
    }

    static void Checkout(Product[] products, int[] cartIds, int[] cartQty, double[] cartSubtotal, int cartCount, string[] orderHistory, ref int orderCount)
    {
        double total = 0;

        for (int i = 0; i < cartCount; i++)
        {
            total += cartSubtotal[i];
        }

        double discount = (total >= 5000) ? total * 0.10 : 0;
        double finalTotal = total - discount;

        double payment;

        while (true)
        {
            Console.Write("Enter payment: ");
            if (double.TryParse(Console.ReadLine(), out payment))
            {
                if (payment >= finalTotal) break;
                Console.WriteLine("Insufficient payment.");
            }
            else Console.WriteLine("Invalid input.");
        }

        double change = payment - finalTotal;

        int receiptNo = new Random().Next(1000, 9999);
        string date = DateTime.Now.ToString();

        Console.WriteLine("\n   RECEIPT     ");
        Console.WriteLine($"Receipt #: {receiptNo}");
        Console.WriteLine($"Date: {date}");

        for (int i = 0; i < cartCount; i++)
        {
            foreach (var p in products)
            {
                if (p.Id == cartIds[i])
                    Console.WriteLine($"{p.Name} x{cartQty[i]} = ₱{cartSubtotal[i]}");
            }
        }

        Console.WriteLine($"Grand Total: ₱{total}");
        Console.WriteLine($"Discount: ₱{discount}");
        Console.WriteLine($"Final Total: ₱{finalTotal}");
        Console.WriteLine($"Payment: ₱{payment}");
        Console.WriteLine($"Change: ₱{change}");

        orderHistory[orderCount++] = $"Receipt {receiptNo} - ₱{finalTotal} - {date}";

        LowStockAlert(products);
    }

    static void LowStockAlert(Product[] products)
    {
        Console.WriteLine("\nLOW STOCK ALERT:");
        foreach (var p in products)
        {
            if (p.RemainingStock <= 5)
                Console.WriteLine($"{p.Name} has only {p.RemainingStock} left.");
        }
    }
}
