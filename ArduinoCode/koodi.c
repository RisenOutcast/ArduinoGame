#define shootPin 2
#define moveFPin 3
#define moveBPin 4
#define moveRPin 5
#define moveLPin 6
#define rotatePin A1

int cannonRotation = 0;
bool shooting = false;
bool moveF, moveB, moveR, moveL = false;

void setup() {
  Serial.begin(9600);
  pinMode(rotatePin, INPUT);
  
  DDRD = DDRD | B00000000; // Set pins 0-7 as input 
  DDRB = DDRB | B00010000; // Set pin 12 to output
  
  attachInterrupt(digitalPinToInterrupt(shootPin), shoot, CHANGE);
}

void loop() {
  rotateCannon();
  printData();
  ledControl();
  
  delay(500);
}

void movement() {
  moveF = digitalRead(moveFPin);
  moveB = digitalRead(moveBPin);
  moveR = digitalRead(moveRPin);
  moveL = digitalRead(moveLPin);
}

void rotateCannon() {
  cannonRotation = analogRead(rotatePin) / 2;
  if(cannonRotation > 360) {
    cannonRotation = 360;
  }
}

void shoot() {
  shooting = !shooting;
}

void printData() {      //Print format: /Shoot;CannonRotation;MoveForward;MoveBack;MoveLeft;MoveRight/
    Serial.print("/");
    Serial.print(shooting);
    Serial.print(";");
    Serial.print(cannonRotation);
    Serial.print(";");
    Serial.print(moveF);
    Serial.print(";");
    Serial.print(moveB);
    Serial.print(";");
    Serial.print(moveR);
    Serial.print(";");
    Serial.print(moveL);
    Serial.print("/");
    Serial.println();
}

void ledControl() {
  PORTB ^= 1 << 4; // Toggle pin 12 state
}