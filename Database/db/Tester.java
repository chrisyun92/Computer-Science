package db;

/**
 * Created by chris on 3/1/2017.
 */
public class Tester {
    public static void main(String[] args) {
        Database lol = new Database();
        System.out.println(lol.transact("load t1"));
        System.out.println(lol.transact("load t2"));
    }
}
