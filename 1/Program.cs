// See https://aka.ms/new-console-template for more information

List<int> depthMeasurements = new List<int>();
parseInput("Input.txt", depthMeasurements);
int count = 0;


int prevAvg = depthMeasurements[0] + depthMeasurements[1] + depthMeasurements[2];

for(int i=1; i<depthMeasurements.Count-2;  i++)
{    
    int avg = depthMeasurements[i]+depthMeasurements[i+1]+depthMeasurements[i+2];
    if (avg > prevAvg)
        count++;
    prevAvg = avg;
}
Console.WriteLine(count);

void parseInput(string filename, List<int> depth)
{
    string fileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\", filename);
    var text = File.ReadLines(fileLocation);

    foreach (string line in text)
    { 
        depth.Add(int.Parse(line));
    }
}