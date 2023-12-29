import numpy as np
from scipy import optimize
import operator

parsed = []

with open("./Input/Day24-example.txt", 'r') as file:
    textLines = file.readlines()
    for line in textLines:
        numsSplit, valsSplit = line.split('@')
        numbers = [int(num.strip()) for num in numsSplit.split(',')]
        dirs = [int(val.strip()) for val in valsSplit.split(',')]
        parsed.append((numbers, dirs))

class Line:
    def __init__(self, start, dir):
        self.start = start
        self.dir = dir

    @staticmethod
    def fromPoints(fr, to, time):
        start = fr
        dir = [coord/time for coord in map(operator.sub, fr, to)]
        return Line(start, dir)
    
    @property
    def a(self):
        return self.dir[1]/self.dir[0] if self.dir[0] != 0 else float('inf')

    @property
    def b(self):
        return self.start[1] - self.a * self.start[0]
    
    def intersect(self, line):
        return ((line.b - self.b) / (self.a - line.a), self.a * (line.b - self.b) / (self.a - line.a) + self.b) if self.a - line.a != 0 else (float('inf'),float('inf'))

    def timeTo(self, point):
        return (point[0] - self.start[0])/self.dir[0]
    
    def at(self, time):
        return [coord + time * dir for coord, dir in zip(self.start, self.dir)]
    

def part2(time1, time2):
    lines = [Line(parsed[i][0], parsed[i][1]) for i in range(len(parsed))]
    base = lines[0]
    target = lines[1]
    guess = Line.fromPoints(base.at(time1), target.at(time1 + time2), time2)
    for i in range(2,len(lines)):
        inter = guess.intersect(lines[i])
        timeto = lines[i].timeTo(inter)
        baseTimeTo = guess.timeTo(inter)
        print(timeto, min(abs(timeto - time1 + baseTimeTo), abs(timeto - time1 - baseTimeTo)))
    # print(base.start, base.dir)
    # print(target.start, target.dir)
    # print(guess.start, guess.dir)
    # print(guess.at(0))
    # print(guess.at(time2))
    return 0

lines = [Line(parsed[i][0], parsed[i][1]) for i in range(len(parsed))]
def f(time1, time2):
    error = 0
    base = lines[0]
    target = lines[1]
    guess = Line.fromPoints(base.at(time1), target.at(time1 + time2), time2)
    for i in range(2,len(lines)):
        inter = guess.intersect(lines[i])
        timeto = lines[i].timeTo(inter)
        baseTimeTo = guess.timeTo(inter)
        error += min(abs(timeto - time1 + baseTimeTo), abs(timeto - time1 - baseTimeTo))
    # print(base.start, base.dir)
    # print(target.start, target.dir)
    # print(guess.start, guess.dir)
    # print(guess.at(0))
    # print(guess.at(time2))
    print(time1, time2, error)
    return error

def fc(vec): 
    return f(vec[0], vec[1])

root = optimize.newton(fc, x0=[4.90,-2], maxiter=500000)
print(root)

# for numbers, values in lines:
#     print("Numbers:", numbers)
#     print("Values:", values)
#     print()

# print(part1())
#print(part2(5,-2))




# Example usage:
def part1():
    count = 0
    for i in range(len(parsed)):
        for j in range(i+1, len(parsed)):
            if i < j:
                #print(lines[i][0], lines[i][1])
                line1 = Line(parsed[i][0], parsed[i][1])
                line2 = Line(parsed[j][0], parsed[j][1])
                #print(line1.a, line2.a)
                intersection = line1.intersect(line2)
                time1 = line1.timeTo(intersection)
                time2 = line2.timeTo(intersection)
                if time1 > 0 and time2 > 0 and 7 <= intersection[0] <= 27 and 7 <= intersection[1] <= 27:
                    count += 1
                if(time1 > 0 and time2 > 0 and 200_000_000_000_000 <= intersection[0] <= 400_000_000_000_000 and 200_000_000_000_000 <= intersection[1] <= 400_000_000_000_000) :
                    count += 1
    return count



