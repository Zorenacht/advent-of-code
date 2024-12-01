import os
import sys
import posixpath
import requests
from dotenv import dotenv_values


def get_input(year: str, day: str, session: str) -> str:
  cookie = {"session": session}
  print("│  ├─ Retrieving input")
  input = requests.get(
    f"https://adventofcode.com/{year}/day/{day}/input",
    cookies=cookie,
    headers={
      "User-Agent": "Manual input retrieval script, https://github.com/renzo-baasdam/advent-of-code/init.py"
    },
  ).text
  return input


def file_exists(dir: str, filename: str) -> bool:
  file_path = posixpath.join(dir, filename)
  if os.path.exists(file_path):
    print(f"│  └─ File already exists: {file_path}")
    return True
  else:
    print("│  ├─ File does not exist, continuing step")
    return False


def get_file_content(dir: str, filename: str, year: str, day: str) -> str:
  file_path = posixpath.join(dir, filename)
  print(f"│  ├─ Retrieving template: {file_path}")
  if os.path.exists(file_path):
    with open(file_path, "r") as template_file:
      return (
        template_file.read()
        .replace("YearPlaceholder", year)
        .replace("DayPlaceholder", day.zfill(2))
      )
  else:
    print(f"│  └─ File does not exist: {file_path}")
    sys.exit(1)


def create_file(dir: str, filename: str, content: str, overwrite=False):
  file_path = posixpath.join(dir, filename)
  print(f"│  ├─ Creating new file: {file_path}")
  os.makedirs(dir, exist_ok=True)
  if overwrite or not os.path.exists(file_path):
    with open(file_path, "w") as f:
      f.write(content)
    print("│  │  └─ Succeeded")
  else:
    print("│  │  └─ Failed: File already exists")


def get_args():
  # Verify arguments
  if len(sys.argv) != 3:
    print("Error: Expected arguments [year] [day of month]")
    sys.exit(1)
  year = sys.argv[1]
  day = sys.argv[2]
  if not year.isdigit():
    print(f"Error: Year {year} must be number.")
    sys.exit(1)
  if not day.isdigit():
    print(f"Error: Day {day} must be a number.")
    sys.exit(1)
  return year, day


try:
  config = dotenv_values(".env")
  year, day = get_args()
  aoc_dir = "./src/AoC"

  # Create input files
  print("├─ Creating input files")
  input_dir = f"{aoc_dir}/{year}/Input"
  input_filename = f"Day{day.zfill(2)}.txt"
  input_example_filename = f"Day{day.zfill(2)}-example.txt"
  if not file_exists(input_dir, input_filename):
    content = get_input(year, day, config.get("session"))
    create_file(input_dir, input_filename, content)
    create_file(input_dir, input_example_filename, "")

  # Create .cs file
  print("├─ Creating .cs file")
  template_dir = aoc_dir
  template_filename = "DayTemplate.cs"
  cs_dir = f"{aoc_dir}/{year}/"
  cs_filename = f"Day{day.zfill(2)}.cs"
  content = get_file_content(template_dir, template_filename, year, day)
  create_file(cs_dir, cs_filename, content)
  print("└─ Completed successfully")
except Exception as e:
  print(f"Error: {e}")
  sys.exit(1)
