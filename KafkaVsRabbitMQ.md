<div dir="rtl" align="right">

# השוואה בין Kafka ל-RabbitMQ

## ארכיטקטורה של המערכת

### Kafka
Kafka עובדת עם topics כאשר כל topic מחולק לאחד או יותר מחיצות (partitions), מה שמאפשר קריאה מקבילית מכל מחיצה על ידי צרכן נפרד. התוכנה הזו עוזרת ל-Kafka להתמודד עם עומסים. חלוקת התור למחיצות ב-Kafka מאפשרת קריאה מקבילית אמיתית, אך בעוד שבתוך המחיצה יש שמירה של סדר ההודעות, בין מחיצות באותו תור אין שמירה על סדר ההודעות.

### RabbitMQ
ב-RabbitMQ אין חלוקה למחיצות; כל תור מנוהל כיחידה אחת. כאשר קבוצה של צרכנים רוצה לקרוא מהתור, כל צרכן מקבל הודעה אחת אחרי השני.

## שמירת הודעות

### Kafka
Kafka שומרת את ההודעות לאחר הקריאה ולא מוחקת אותן מיד. מחיקת הודעות מתבצעת בהתאם למדיניות זמן ומקום בדיסק.

### RabbitMQ
RabbitMQ מוחקת את ההודעה מיד לאחר שההודעה נקראה. אם המערכת רוצה לאפשר להודעות להיקרא יותר מפעם אחת (לדוגמה, לצורכי אנליזה), Kafka תתאים יותר.

## עמידות וזמינות של המערכת

### Kafka
Kafka מכילה מנגנון מובנה שמשכפל מחיצות בין כמה שרתים. כל מחיצה מנוהלת על ידי Kafka במודל leader ו-follower, כך שהכתיבה מתבצעת קודם ל-leader ולאחר מכן נשלח עותק ל-follower.

### RabbitMQ
גם RabbitMQ תומך בעמידות של התורים ויש לו מנגנון שנקרא mirror queue, אך נדרשת הגדרה מיוחדת לכך.

## ניתוב

### Kafka
ב-Kafka הניתוב הוא לפי שם של topic בלבד. הניתוב פשוט וללא מורכבות.

### RabbitMQ
ב-RabbitMQ, כאשר שולחים הודעה, לא כותבים לאיזה topic אלא לאיזה exchange. ה-exchange מכיר את חוקיות הניתוב ויודע לאיזה תור לנתב את ההודעה. יש exchanges שעובדים לפי מפתח הודעה, כאלה ששולחים לקבוצה של תורים, וכאלה לפי תבנית מסוימת. ניתן להגדיר ניתוב חכם ומורכב.

## הודעות שגויות

### Kafka
כאשר נתקלים בהודעה בעייתית שלא הצלחנו לטפל בה, ניתן לשמור את ה-offset שלה ואחר כך לחזור ולקרוא אותה.

### RabbitMQ
ב-RabbitMQ לא ניתן לקרוא מ-offset מסוים, אולם יש מנגנון מובנה שנקרא DLQ (Dead Letter Queue), שהוא למעשה תור להודעות מתות. הודעה תישלח לתור הזה אוטומטית כאשר הצרכן מחזיר הודעת nack לאחר שהוא משך את ההודעה מהתור.

## ניהול זיכרון וביצועים

### Kafka
ב-Kafka ההודעות שנכתבות לדיסק, ולכן יש תקורה של זמן.

### RabbitMQ
ב-RabbitMQ ההודעות נשמרות ל-RAM ורק אחר כך יש מנגנון שמעביר אותן לדיסק.

Kafka מתאים לכמויות גדולות של הודעות (מיליונים בשנייה) או לסטרים (מקרים שבהם הודעות מגיעות ללא הפסקה).

## תאימות ואינטגרציה עם מערכות אחרות

### Kafka
אם המערכת שלנו עובדת עם Kafka, לא ניתן להחליף את Kafka במערכת אחרת, כי הקוראים והכותבים משתמשים בפרוטוקול שרק Kafka מדברת אותו.

### RabbitMQ
לעומת זאת, כאשר עובדים עם RabbitMQ, ניתן להחליף אותו במערכת אחרת, כי הוא מממש פרוטוקול גנרי. לדוגמה, יש חיישנים שקוראים טמפרטורה ולחץ וכו' שיכולים להתחבר למיקרו-בקרים שמפיצים את ההודעות שלהם בפרוטוקול ש-RabbitMQ תומך בו. כך, לא נצטרך לכתוב מתווך שקורא את הנתונים מהם ושולח ל-RabbitMQ.

## עדיפות

### Kafka
ב-Kafka אין אפשרות לקבוע עדיפות להודעות. כל ההודעות נקראות לפי סדר ההכנסה שלהן ואין הודעה שיכולה להיקרא לפני הודעה אחרת.

### RabbitMQ
ב-RabbitMQ ניתן להגדיר עדיפות להודעות בתוך אותו תור. חשוב לשים לב מתי כדאי להגדיר תור אחד עם עדיפות להודעות בתוך התור לעומת שני תורים (דחוף ולא דחוף). כאשר יש שני תורים, ההודעות נצרכות משני התורים במקביל, כך שהטיפול בהודעות הדחופות הוא באמת מקבילי. הודעות דחופות באותו התור זה יותר ל"עדיפות" ולא לדחיפות.

## דחיסה של הודעות בעלות אותו מפתח

Kafka מאפשרת דחיסת הודעות לפי מפתח הודעה, מה שעוזר בניהול ושיפור ביצועים של מערכת המנוהלת בעומסים גבוהים.

## סיכום

ההבדלים בין Kafka ל-RabbitMQ הם מהותיים וכוללים את אופן ניהול ההודעות, עמידות וזמינות, ניתוב הודעות, טיפול בהודעות שגויות, ניהול זיכרון וביצועים, תאימות עם מערכות אחרות ויכולות עדיפות. כל מערכת מתאימה לסוגים שונים של עומסים ושימושים, ויש לבחור את המערכת המתאימה ביותר לצרכים הספציפיים של המערכת שלכם.

</div>