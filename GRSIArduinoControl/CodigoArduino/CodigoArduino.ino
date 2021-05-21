#define intervalo 500
#define pot A0
#define ledVerde 2
#define ledAmarelo 3
#define ledVermelho 4
#define buz 5

int leituraPOT=0;
int contador = 0;


void setup() {
  Serial.begin(9600);
  //declarar os pinos 2,3 e 3 como saídas
  pinMode(ledVerde,OUTPUT);
  pinMode(ledAmarelo,OUTPUT);
  pinMode(ledVermelho,OUTPUT);
  pinMode(buz,OUTPUT);
  //desligar os pinos 2,3 e 4 ao iniciar
  digitalWrite(ledVerde,LOW);
  digitalWrite(ledAmarelo,LOW);
  digitalWrite(ledVermelho,LOW);
  digitalWrite(buz,LOW);
  teste();
}
void loop() {
  
  String input="";
  while(Serial.available()){
    char c = Serial.read();
    input += c;
  }

  //comandos LED vermelho
  if(input == "L"){
    digitalWrite(ledVermelho, HIGH);
    Serial.println(digitalRead(ledVermelho));
  }
  if (input == "D") {
    digitalWrite(ledVermelho, LOW);
    Serial.println(digitalRead(ledVermelho));
  }

  //comandos LED amarelo
  if(input == "A"){
    digitalWrite(ledAmarelo, HIGH);
    Serial.println(digitalRead(ledAmarelo));
  }
  if (input == "B") {
    digitalWrite(ledAmarelo, LOW);
    Serial.println(digitalRead(ledAmarelo));
  }

  //comandos LED verde
  if(input == "C"){
    digitalWrite(ledVerde, HIGH);
    Serial.println(digitalRead(ledVerde));
  }
  if (input == "E") {
    digitalWrite(ledVerde, LOW);
    Serial.println(digitalRead(ledVerde));
  }
  
  //pedido de inicalização
  if (input == "I") {
    int statusRed = digitalRead(ledVermelho);
    int statusYellow = digitalRead(ledAmarelo);
    int statusGreen = digitalRead(ledVerde);
    String init = (String)statusRed + ";" + (String)statusYellow + ";" + (String)statusGreen;
    Serial.println(init);
  }

}

void teste(){
  digitalWrite(ledVerde,HIGH);
  digitalWrite(ledAmarelo,HIGH);
  digitalWrite(ledVermelho,HIGH);
  tone(buz,1500,100);
  delay(200);
  tone(buz,2000,100);
  delay(200);
  tone(buz,2500,100);
  delay(200);
  digitalWrite(ledVerde,LOW);
  digitalWrite(ledAmarelo,LOW);
  digitalWrite(ledVermelho,LOW);
  delay(200);
}
