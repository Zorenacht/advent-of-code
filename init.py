import os
import sys
import posixpath

# Verify arguments
if len(sys.argv) != 3:
  print("Error: Expected arguments [year] [day of month]")
  sys.exit(1)
year_str = sys.argv[1]
day_str = sys.argv[2]
if not year_str.isdigit():
  print(f"Error: Year {year_str} must be number.")
  sys.exit(1)
if not day_str.isdigit():
  print(f"Error: Day {day_str} must be a number.")
  sys.exit(1)
year = int(year_str)
day = int(day_str)

# Define the directory and file paths
aoc_dir = posixpath.join("./src/AoC")
year_dir = posixpath.join(aoc_dir, year_str)

template_name = "DayTemplate.cs"
template_path = posixpath.join(aoc_dir, template_name)
file_name = f"Day{day_str.zfill(2)}.cs"
file_path = posixpath.join(aoc_dir, year_str, file_name)

# Create the file
try:
  # Retrieve template
  print(f"├─ Retrieving template: {template_path}")
  if os.path.exists(template_path):
    with open(template_path, "r") as template_file:
      template = (
        template_file.read()
        .replace("YearPlaceholder", year_str)
        .replace("DayPlaceholder", day_str)
      )
  else:
    print(f"│  └─ File does not exist: {template_path}")
    sys.exit(1)

  # Create the new file (and directories if needed)
  print(f"└─ Creating new file: {file_path}")
  os.makedirs(year_dir, exist_ok=True)
  if not os.path.exists(file_path):
    with open(file_path, "w") as f:
      f.write(template)
    print(f"   └─ File created: {file_path}")
  else:
    print(f"   └─ File already exists: {file_path}")
except Exception as e:
  print(f"Error: {e}")
  sys.exit(1)
