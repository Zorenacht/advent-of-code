
[Test]
part2();

void part2()
{
    const int total_generations = 256;
    long[] population = new long[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    Reader.ReadAsText("Input.txt");
    const int normal_internal_timer = 7;
    const int new_internal_timer = normal_internal_timer+2;

    int[] pop_input = new int[] { 2, 1, 2, 1, 5, 1, 5, 1, 2, 2, 1, 1, 5, 1, 4, 4, 4, 3, 1, 2, 2, 3, 4, 1, 1, 5, 1, 1, 4, 2, 5, 5, 5, 1, 1, 4, 5, 4, 1, 1, 4, 2, 1, 4, 1, 2, 2, 5, 1, 1, 5, 1, 1, 3, 4, 4, 1, 2, 3, 1, 5, 5, 4, 1, 4, 1, 2, 1, 5, 1, 1, 1, 3, 4, 1, 1, 5, 1, 5, 1, 1, 5, 1, 1, 4, 3, 2, 4, 1, 4, 1, 5, 3, 3, 1, 5, 1, 3, 1, 1, 4, 1, 4, 5, 2, 3, 1, 1, 1, 1, 3, 1, 2, 1, 5, 1, 1, 5, 1, 1, 1, 1, 4, 1, 4, 3, 1, 5, 1, 1, 5, 4, 4, 2, 1, 4, 5, 1, 1, 3, 3, 1, 1, 4, 2, 5, 5, 2, 4, 1, 4, 5, 4, 5, 3, 1, 4, 1, 5, 2, 4, 5, 3, 1, 3, 2, 4, 5, 4, 4, 1, 5, 1, 5, 1, 2, 2, 1, 4, 1, 1, 4, 2, 2, 2, 4, 1, 1, 5, 3, 1, 1, 5, 4, 4, 1, 5, 1, 3, 1, 3, 2, 2, 1, 1, 4, 1, 4, 1, 2, 2, 1, 1, 3, 5, 1, 2, 1, 3, 1, 4, 5, 1, 3, 4, 1, 1, 1, 1, 4, 3, 3, 4, 5, 1, 1, 1, 1, 1, 2, 4, 5, 3, 4, 2, 1, 1, 1, 3, 3, 1, 4, 1, 1, 4, 2, 1, 5, 1, 1, 2, 3, 4, 2, 5, 1, 1, 1, 5, 1, 1, 4, 1, 2, 4, 1, 1, 2, 4, 3, 4, 2, 3, 1, 1, 2, 1, 5, 4, 2, 3, 5, 1, 2, 3, 1, 2, 2, 1, 4 };
    foreach(int entry in pop_input)
    {
        population[entry] += 1;
    }


    for (int gen=0; gen<total_generations; gen++)
    {
        long new_fishes = population[0];
        Array.Copy(population, 1, population, 0, population.Length - 1);
        population[normal_internal_timer - 1] += new_fishes;
        population[new_internal_timer - 1] = new_fishes;
        Console.WriteLine(String.Join(", ", population));
    }

    Console.WriteLine("Number of fishes after " + total_generations + " generations: " + population.Sum());
}
