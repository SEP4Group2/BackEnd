import subprocess
import sys
from datetime import datetime

def is_ef_installed():
    try:
        subprocess.run(["dotnet", "ef"], stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
        return True
    except FileNotFoundError:
        return False

def install_ef():
    subprocess.run(["dotnet", "tool", "install", "--global", "dotnet-ef"])

def run_migration(migration_name):
    subprocess.run([
        "C:\\Users\\Ola\\.dotnet\\tools\\dotnet-ef", "migrations", "add", migration_name,
        "--project", "../DataAccess/DataAccess.csproj",
        "--startup-project", "../BackEndAPI/BackEndAPI.csproj"
    ])

def main():
    if len(sys.argv) < 2:
        migration_name = "Migration_" + datetime.now().strftime("%Y-%m-%d_%H-%M-%S")
    else:
        migration_name = sys.argv[1]

    if not is_ef_installed():
        print("Entity Framework CLI is not installed. Installing now...")
        install_ef()

    run_migration(migration_name)

if __name__ == "__main__":
    main()