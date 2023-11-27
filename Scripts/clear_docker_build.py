import sys
import subprocess
import argparse
import re

def run_compose_down():
    subprocess.run(["docker", "compose", "down", "-v"])
def run_build_api(new_image_name):
    subprocess.run(["docker", "build", "-t", new_image_name, ".."])
def rename_image_name_in_compose_file(new_image_name):
    composePath = '../docker-compose.yml';
    with open(composePath, 'r') as file:
        content = file.read()

    content = re.sub(r'(services:\s+plantapi:\s+image: ").*?(")', fr'\g<1>{new_image_name}\2', content, flags=re.DOTALL)
    
    with open(composePath, 'w') as file:
        file.write(content)
def run_compose_up():
    subprocess.run(["docker", "compose","-f", "../docker-compose.yml", "up"])

def main():
    parser = argparse.ArgumentParser(description="clean build docker")
    parser.add_argument("image_name", type=str, help="The new API image name")
    args = parser.parse_args()
    
    run_compose_down()
    run_build_api(args.image_name)
    rename_image_name_in_compose_file(args.image_name)
    run_compose_up()
   

if __name__ == "__main__":
    main()