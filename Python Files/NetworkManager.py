import socket
import time

host, port = "192.168.1.41", 25001

def setup_connection():
    global sock
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    try:
        sock.connect((host, port))
    except:
        setup_connection()

def get_file_location():
    global sock
    file_location = ''
    try:
        file_location = sock.recv(1024).decode("UTF-8") #receiveing data in Byte from C#, and converting it to String
    except:
        print("error detected in connection. restablishing connection")
    while file_location == '': # if received string is empty, it could mean the connection is lost
        sock.close()
        setup_connection() # try restablishing connection
        try:
            file_location = sock.recv(1024).decode("UTF-8") #receiveing data in Byte from C#, and converting it to String
        except (SyntaxError, IndexError) as E:
            print(E)

    print(f"received: {file_location}")
    file_content = open(file_location, "r").read()
    send_cube_info(file_content)

def send_cube_info(file_content):
    sock.sendall(file_content.encode("UTF-8")) #Converting string to Byte, and sending it to C#
    # time.sleep(5)
    print("result sent")

setup_connection()
while True:
    get_file_location()