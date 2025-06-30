namespace TourGuide.DataLayer.Repositories
{
    public class SearchRepository
    {
        //Das ist nur mal so ungefähr wie die Suche aussehen könnte dann. Wenn wir die DB fertig haben müssen wir noch
        //kontrollieren ob die Namen dann alle auch übereinstimmen und das natürlich auch funktioniert.
        //Auch nochmal Datenbanksachen genau wegen SQL-Injections durchgehen - die Inputprüfung mach ich aber in
        //searchviewmodel bevor ich es übergebe.
        //Derweil kommentier ich das aber aus, damit es keine Probleme gibt.
        
        //Müssen wir aber später eh noch ändern weil zurzeit gibts ja nur den SQL-Command zurück, aber wir wollen ja
        //Daten aus der Datenbank zurückbekommen.
        
        
        /*public string BuildSearchQuery(string searchinput)
        {
            return $@"
            SELECT * FROM tour
            WHERE name ILIKE '%{searchinput}%' OR
                  description ILIKE '%{searchinput}%' OR
                  startLocation ILIKE '%{searchinput}%' OR
                  endLocation ILIKE '%{searchinput}%' OR
                  transporttype ILIKE '%{searchinput}%' OR
                  CAST(distance AS TEXT) ILIKE '%{searchinput}%' OR
                  CAST(estimatedTime AS TEXT) ILIKE '%{searchinput}%'
            UNION
            SELECT * FROM tourlog
            WHERE comment ILIKE '%{searchinput}%' OR
                  date ILIKE '%{searchinput}%' OR
                  CAST(difficulty AS TEXT) ILIKE '%{searchinput}%' OR
                  CAST(totalTime AS TEXT) ILIKE '%{searchinput}%' OR
                  CAST(rating AS TEXT) ILIKE '%{searchinput}%'
            ";
        }
        */

    }
}