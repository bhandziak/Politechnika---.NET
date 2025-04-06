namespace Prototype_Project
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Owner janKowalski = new Owner("Jan", "Kowalski");
            Animal animal = new Animal("Azor", 4, janKowalski);
            Cat cat = new Cat("Puszek", 6,janKowalski, "Norweski leśny");

            // kopia płytka
            Animal animalCopy = animal.ShallowClone();

            animalCopy.Age = 1;
            animalCopy.Name = "Reks";
            animalCopy.Owner.Name = "Staszek";

            // kopia głęboka
            Animal animalDeepCopy = animal.DeepClone();

            animalDeepCopy.Age = 1;
            animalDeepCopy.Name = "Reks";
            animalDeepCopy.Owner.Name = "Andrzej";

            // przykład dla klasy dziedzicznej

            Cat catCopy = cat.ShallowClone();
            catCopy.Age = 2;
            catCopy.Name = "Kicia";
            catCopy.Breed = "Mainkun";


            Console.WriteLine("Orginał animal: ");
            Console.WriteLine(animal);

            Console.WriteLine("Kopia płytka animal: ");
            Console.WriteLine(animalCopy);

            Console.WriteLine("Kopia głęboka animal: ");
            Console.WriteLine(animalDeepCopy);

            Console.WriteLine();

            Console.WriteLine("Orginał cat: ");
            Console.WriteLine(cat);
            Console.WriteLine("Kopia płytka cat: ");
            Console.WriteLine(catCopy);
        }
    }
}
