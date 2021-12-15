#define shootPin 2
#define moveLPin 3
#define moveRPin 4
#define moveFPin 5
#define moveBPin 6
#define rotatePin A1
#define collectPin3 A0
#define collectPin2 A2
#define collectPin1 A5

int cannonRotation = 0;
bool shooting = false;
bool moveF, moveB, moveR, moveL = false;

int health = 3;
int collectibles = 0;
bool onEMP = false;
int onEMPTime = 0;
bool empToggle = false;

int incomingByte = 0;

void setup() {
  Serial.begin(9600);

  // Asetetaan pinnit sisään-/ulostuloiksi
  pinMode(rotatePin, INPUT);
  
  pinMode(collectPin1, OUTPUT);
  pinMode(collectPin2, OUTPUT);
  pinMode(collectPin3, OUTPUT);

  DDRD = DDRD | B00000000; // Asetetaan pinnit 0-7 sisääntuloiksi 
  DDRB = DDRB | B00011100; // Asetetaan pinnit 12, 11, 10 ulostuloiksi

  attachInterrupt(digitalPinToInterrupt(shootPin), shoot, CHANGE);  // Lisätään keskeytyspalvelu ampumisnappiin

  cli();  // Pysäytetään keskeytykset

  //set timer1 interrupt at 1Hz
  TCCR1A = 0; // set entire TCCR1A register to 0
  TCCR1B = 0; // same for TCCR1B
  TCNT1  = 0; //initialize counter value to 0
  // set compare match register for 1hz increments
  OCR1A = 15624;  // = (16*10^6) / (1*1024) - 1 (must be <65536)
  // turn on CTC mode
  TCCR1B |= (1 << WGM12);
  // Set CS12 and CS10 bits for 1024 prescaler
  TCCR1B |= (1 << CS12) | (1 << CS10);  
  // enable timer compare interrupt
  TIMSK1 |= (1 << OCIE1A);

  sei();  // Keskeytykset takaisin päälle

  resetGame();  // Resetoidaan pelin alkutilanteeseen
} 

void loop() {
  if(!onEMP) {
    rotateCannon();
    movement();
    printData();
  }

  if (Serial.available() > 0) {
    // Luetaan sisään tuleva data
    incomingByte = Serial.read();
    switch (incomingByte) {
      case 48:
        resetGame();
        break;
      case 49:
        health -= 1;
        healthLeds();
        break;
      case 50:
        health += 1;
        healthLeds();
        break;
      case 51:
        collectibles += 1;
        collectibleLeds();
        break;
      case 52:
        emp();
        break;
      default:
        break;
    }
  }

  delay(300);
}

void resetGame() {  // Resetoidaan arduino pelin alkutilanteeseen
  health = 3;
  collectibles = 0;
  onEMP = false;
  onEMPTime = 0;
  
  healthLeds();
  collectibleLeds();
}

ISR(TIMER1_COMPA_vect){ // Timer1 1Hz keskeytys
  if(onEMP) {
    if(onEMPTime >= 5) {
      onEMP = false;
      onEMPTime = 0;
      healthLeds();
      collectibleLeds();
    }
    else {
      if(empToggle) {
        PORTB |= 1 << 4; // Ledi pinnissä 12 poispäältä
        PORTB |= 1 << 3; // Ledi pinnissä 11 poispäältä
        PORTB |= 1 << 2; // Ledi pinnissä 10 poispäältä
        digitalWrite(collectPin1, HIGH);
        digitalWrite(collectPin2, HIGH);
        digitalWrite(collectPin3, HIGH);
        empToggle = false;
      }
      else {
        PORTB &= ~(1 << 4); // Ledi pinnissä 12 päälle
        PORTB &= ~(1 << 3); // Ledi pinnissä 11 päälle
        PORTB &= ~(1 << 2); // Ledi pinnissä 10 päälle
        digitalWrite(collectPin1, LOW);
        digitalWrite(collectPin2, LOW);
        digitalWrite(collectPin3, LOW);
        empToggle = true;
      }
      onEMPTime += 1;
    }
  }
  else if(health == 1) {  // Jos health = 1, viimeinen ledi vilkkuu
    PORTB ^= 1 << 2;
  }
}

void emp() {
  onEMPTime = 0;
  onEMP = true;
}

void healthLeds() {
  if(health > 3) {
    health = 3;
  }
  if(health >= 3) {
    PORTB &= ~(1 << 4); // Ledi pinnissä 12 päälle
    PORTB &= ~(1 << 3); // Ledi pinnissä 11 päälle
    PORTB &= ~(1 << 2); // Ledi pinnissä 10 päälle
  }
  else if(health == 2) {
    PORTB |= 1 << 4; // Ledi pinnissä 12 poispäältä
    PORTB &= ~(1 << 3); // Ledi pinnissä 11 päälle
    PORTB &= ~(1 << 2); // Ledi pinnissä 10 päälle
  }
  else if(health == 1) {
    PORTB |= 1 << 4; // Ledi pinnissä 12 poispäältä
    PORTB |= 1 << 3; // Ledi pinnissä 11 poispäältä
    PORTB &= ~(1 << 2); // Ledi pinnissä 10 päälle
  }
  else {
    PORTB |= 1 << 4; // Ledi pinnissä 12 poispäältä
    PORTB |= 1 << 3; // Ledi pinnissä 11 poispäältä
    PORTB |= 1 << 2; // Ledi pinnissä 10 poispäältä
  }
}

void collectibleLeds() {
  if(collectibles >= 3) {
    digitalWrite(collectPin1, LOW);
    digitalWrite(collectPin2, LOW);
    digitalWrite(collectPin3, LOW);
  }
  else if(collectibles == 2) {
    digitalWrite(collectPin1, LOW);
    digitalWrite(collectPin2, LOW);
    digitalWrite(collectPin3, HIGH);
  }
  else if(collectibles == 1) {
    digitalWrite(collectPin1, LOW);
    digitalWrite(collectPin2, HIGH);
    digitalWrite(collectPin3, HIGH);
  }
  else {
    digitalWrite(collectPin1, HIGH);
    digitalWrite(collectPin2, HIGH);
    digitalWrite(collectPin3, HIGH);
  }
}

void movement() {
  moveF = digitalRead(moveFPin);
  moveB = digitalRead(moveBPin);
  moveR = digitalRead(moveRPin);
  moveL = digitalRead(moveLPin);
}

void rotateCannon() {
  cannonRotation = analogRead(rotatePin) / 2; // Muutetaan potikan lukema välille 0-360 unityä varten
  if(cannonRotation > 360) {
    cannonRotation = 360;
  }
}

void shoot() {  // Napin keskeytyspalvelu
  shooting = digitalRead(2);
}

void printData() {      //Print format: /Shoot;CannonRotation;MoveForward;MoveBack;MoveRight;MoveLeft/
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