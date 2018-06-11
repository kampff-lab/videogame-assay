int inputPin = 4;
int outputPin = 6;
int in = 0;

void setup() {
  // put your setup code here, to run once:
  pinMode(inputPin, INPUT);
  pinMode(outputPin, OUTPUT);
}

void loop() {
  // put your main code here, to run repeatedly:
  in = digitalRead(inputPin);
  digitalWrite(outputPin, in);
}
