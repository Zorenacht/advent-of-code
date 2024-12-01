import sys
import subprocess
import os

# Determine the command to run based on the platform
os.environ["PIPENV_VERBOSITY"] = "-1"
command = ["pipenv", "run", "python", "init.py"] + sys.argv[1:]
subprocess.run(command)
