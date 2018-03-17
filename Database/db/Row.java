package db;

import java.util.ArrayList;

/**
 * Created by chris on 2/18/2017.
 */
public class Row {
    public ArrayList singleRow;
    public String rowName;

    public Row(String name) {
        rowName = name;
        singleRow = new ArrayList<>();
    }

    public void add(String...strings) {
        for (String b: strings) {
            singleRow.add(b);
        }
    }

    public void add(Integer...ints) {
        for (Integer b: ints) {
            singleRow.add(b);
        }
    }

    public void add(Float...floats) {
        for (Float b: floats) {
            singleRow.add(b);
        }
    }
    public Object get(int x) {
        return singleRow.get(x);
    }

    public String printName() {
        return this.rowName;
    }

    public Row makeIntoOneRow(Row r2) {
        for (int x = 0; x < r2.singleRow.size(); x++) {

        }
        return this;
    }
}
