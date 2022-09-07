import socket
import time

host, port = "192.168.1.41", 25001

def setup_connection():
    global sock
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sock.connect((host, port))

def get_file_location():
    global sock
    file_location = ''
    file_location = sock.recv(1024).decode("UTF-8") #receiveing data in Byte from C#, and converting it to String
    while file_location == '': # if received string is empty, it could mean the connection is lost
        sock.close()
        setup_connection() # try restablishing connection
        file_location = sock.recv(1024).decode("UTF-8") #receiveing data in Byte from C#, and converting it to String

    print(f"received: {file_location}")
    file_content = open(file_location, "r").read()
    print(file_content)
    send_cube_info(file_content)

def send_cube_info(file_content):
    sock.sendall(file_content.encode("UTF-8")) #Converting string to Byte, and sending it to C#
    # time.sleep(5)
    print("result sent")

setup_connection()
while True:
    get_file_location()