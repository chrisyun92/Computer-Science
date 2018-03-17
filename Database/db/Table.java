package db;

import java.util.ArrayList;
import java.util.TreeSet;

/**
 * Created by chris on 2/18/2017.
 */
public class Table {
    public String tableName;
    public ArrayList<Column> cols;
    private ArrayList<Row> rows;
    public Coordinates noValues;

    public Table(String tableName) {
        this.tableName = tableName;
        cols= new ArrayList<>();
        rows= new ArrayList<>();
        noValues = new Coordinates();
    }

    public void insertCol(Column col) {
        cols.add(col);
    }

    public int getColLen() {
        return cols.size();
    }

    public ArrayList<Column> getCol() {
        return cols;
    }

    public static ArrayList<String> findCommonCols(Table first, Table second) {
        ArrayList<String> commonCols = new ArrayList<>();
        for (int x = 0; x < first.cols.size(); x++) {
            for (int i = 0; i < second.cols.size(); i++) {
                if (first.cols.get(x).printName().equals(second.cols.get(i).printName())) {
                    commonCols.add(first.cols.get(x).printName());
                }
            }
        }
        return commonCols;
    }

    public ArrayList<Column> getActualCommonCols(Table first, Table second) {
        ArrayList<String> fCC = findCommonCols(first, second);
        ArrayList<Column> ret = new ArrayList<>();
        for (String s: fCC) {
            ret.add(Table.getColFromString(s, this));
        }
        return ret;
    }

    public static boolean Inside(Column c, ArrayList<String> cols) {
        for (String col: cols) {
            if (c.printName().equals(col)) {
                return true;
            }
        }
        return false;

    }

    public static PairsOfIndices distill(Table first, Table second) {
        ArrayList<String> commonCols = findCommonCols(first, second);
        ArrayList<Column> commonColsInT1 = new ArrayList<>();
        for (String commonColNames: commonCols) {
            commonColsInT1.add(getColFromString(commonColNames, first));
        }
        ArrayList<Column> commonColsInT2 = new ArrayList<>();
        for (String commonColNames: commonCols) {
            commonColsInT2.add(getColFromString(commonColNames, second));
        }
        PairsOfIndices result = new PairsOfIndices();
        for (int index = 0; index < commonColsInT1.size(); index ++) {
            PairsOfIndices toAdd = Column.twoColumnCommonRows(commonColsInT1.get(index), commonColsInT2.get(index));
            result.append(toAdd);
        }
        return result;
    }

    public static PairsOfIndices indicesToUseDuringJoins (Table first, Table second) {
        ArrayList<String> commonCols = findCommonCols(first, second);
        ArrayList<Column> commonColsInT1 = new ArrayList<>();
        for (String commonColNames: commonCols) {
            commonColsInT1.add(getColFromString(commonColNames, first));
        }
        ArrayList<Column> commonColsInT2 = new ArrayList<>();
        for (String commonColNames: commonCols) {
            commonColsInT2.add(getColFromString(commonColNames, second));
        }

        PairsOfIndices distilled = distill(first, second);
        int requirement = findCommonCols(first,second).size();
        ArrayList<Integer> uniqueIndicesD1 = obtainUniqueIndices(distilled.table1Indices());

        ArrayList<Integer> uniqueIndicesD2 = obtainUniqueIndices(distilled.table2Indices());

        ArrayList<Integer> keptValuesD1 = new ArrayList<>();
        for (Integer uID1entry: uniqueIndicesD1) {
            int count = 0;
            for (Integer a: distilled.table1Indices() ) {
                if (uID1entry.equals(a)) {
                    count ++;
                }
                if (count == requirement) {
                    keptValuesD1.add(uID1entry);
                    break;
                }
            }
        }

        ArrayList<Integer> keptValuesD2 = new ArrayList<>();
        for (Integer uID2entry: uniqueIndicesD2) {
            int count = 0;
            for (Integer a: distilled.table2Indices() ) {
                if (uID2entry.equals(a)) {
                    count ++;
                }
                if (count == requirement) {
                    if (Table.checkIfRowInC2MatchesAnyInC1(commonColsInT1, commonColsInT2, uID2entry)) {
                        keptValuesD2.add(uID2entry);
                    }
                    break;
                }
            }
        }

        return new PairsOfIndices(keptValuesD1, keptValuesD2);
    }

    public static boolean checkIfRowInC2MatchesAnyInC1(ArrayList<Column> c1, ArrayList<Column> c2, int rowNum) {
        ArrayList row2 = new ArrayList();
        for (Column col2: c2) {
            row2.add(col2.get(rowNum));
        }
        for (int index = 0; index < c2.get(0).size(); index ++) {
            ArrayList row1 = new ArrayList();
            for (Column col1: c1) {
                row1.add(col1.get(index));
            }
            if (equalArrayLists(row1, row2)) {
                return true;
            }
        }
        return false;
    }

    public static ArrayList<Integer> obtainUniqueIndices(ArrayList<Integer> input) {
        ArrayList<Integer> ret = new ArrayList<>(new TreeSet<Integer>(input));
        return ret;
    }

    public static boolean equalArrayLists(ArrayList a1, ArrayList a2) {
        for (int a = 0; a < a1.size(); a++) {
            if (! a1.get(a).equals(a2.get(a))) {
                return false;
            }
        }
        return true;
    }


    public static ArrayList<Column> uniqueColumnsToTable2 (Table first, Table second) {
        ArrayList<Column> ret = new ArrayList<>();
        for (Column c: second.cols) {
            if (! Inside(c, findCommonCols(first, second))) {
                ret.add(c);
            }
        }
        return ret;
    }


    public static Column getColFromString(String colName, Table table) {
        for (int i = 0; i < table.cols.size(); i++) {
            if (table.cols.get(i).printName().equals(colName)) {
                return table.cols.get(i);
            }
        }
        return null;
    }

    public static Table getRowsOfATable (Table t, ArrayList<Integer> rows) {
        Table ret = new Table(t.tableName);
        for (Column c: t.getCol()) {
            ret.insertCol(c.obtainPartsOfColumnEntriesForJoins(rows));
        }
        return ret;
    }

    public static Table joins(Table first, Table second) {
        if (findCommonCols(first, second).size() == 0) {
            return cartesianJoins(first,second);
        }
        ArrayList<Integer> firstTableJoinIndices = indicesToUseDuringJoins(first,second).table1Indices();

        ArrayList<Integer> secondTableJoinIndices = indicesToUseDuringJoins(first,second).table2Indices();

        //Table newFirst = new Table("newFirst");

        ArrayList<Table> tablesToStack = new ArrayList<>();
        for (Integer in: firstTableJoinIndices) {
            ArrayList<Integer> singleRow = new ArrayList<>();
            singleRow.add(in);
            Table rowOfFirstTable = Table.getRowsOfATable(first, singleRow);
            ArrayList bodyOfThisRow = new ArrayList();
            for (Column c: first.getActualCommonCols(first, second)) {
                bodyOfThisRow.add(c.singleColumn.get(in));
            }
            Table secondTable = new Table(second.tableName);
            ArrayList<Column> secondCommonCols = second.getActualCommonCols(first, second);
            secondCommonCols = Column.obtainPartsOfColumnsThatAreThisRow(secondCommonCols,
                    uniqueColumnsToTable2(first, second), bodyOfThisRow);
            secondTable.cols = secondCommonCols;
            Table toStack = cartesianJoins(rowOfFirstTable, secondTable);
            tablesToStack.add(toStack);
        }
        Table result = tablesToStack.get(0);
        for (int index = 1; index < tablesToStack.size(); index ++) {
            result = Table.stackTwoTables(result, tablesToStack.get(index));
        }
        return result;



        /*for (Column c: first.cols) {
            newFirst.cols.add(c.obtainPartsOfColumnEntriesForJoins(firstTableJoinIndices));
        }
        Table newSecond = new Table("newSecond");
        for (Column c: uniqueColumnsToTable2(first, second)) {
            newSecond.cols.add(c.obtainPartsOfColumnEntriesForJoins(secondTableJoinIndices));
        }
        return cartesianJoins(newFirst, newSecond);*/

    }



    public static Table cartesianJoins(Table first, Table second) {
        if (first.cols.size() == 0) {
            return second;
        }
        else if (second.cols.size() == 0) {
            return first;
        }
        Table newTable = new Table("new");
        for (Column c1: first.cols) {
            Column newCol = new Column (c1.printName(), c1.printType());
            for (Object entry: c1.singleColumn) {
                for (int a = 0; a < second.cols.get(0).size(); a++) {
                    newCol.add(entry);
                }
            }
            newTable.insertCol(newCol);
        }
        for (Column c2: second.cols) {
            Column newCol = new Column (c2.printName(), c2.printType());
            int inc = 0;
            int index = 0;
            while (inc < first.cols.get(0).size()) {
                newCol.add(c2.singleColumn.get(index % second.cols.get(0).size()));
                index ++;
                if (index % second.cols.get(0).size() == 0) {
                    inc ++;
                }
            }
            newTable.insertCol(newCol);
        }
        return newTable;
    }

    public static Table stackTwoTables(Table t1, Table t2) {
        Table ret = new Table("anonymous");
        for (int index = 0; index < t1.getColLen(); index ++) {
            ret.insertCol(Column.stackColumns(t1.cols.get(index), t2.cols.get(index)));
        }
        return ret;
    }

    public String printName() {
        return tableName;
    }

    public int columnIndex(Column c) {
        int index = 0;
        for (Column col: this.cols) {
            if (col.printName().equals(c.printName())) {
                return index;
            }
            index ++;
        }
        return index;
    }

    public Object getElement(int x, int y) {
        Column col = this.cols.get(x);
        Object element = col.get(y);
        if (element instanceof Float) {
            return String.format("%.3f", element);
        }
        return element;
    }

    public String printContents() {
        String ans = "";
        if (cols.size() == 1) {
            ans += cols.get(0).printName() + " " + cols.get(0).getType() + "\n";
            for (int x = 0; x < cols.get(0).size(); x++) {
                ans += getElement(0, x) + "\n";
            }
        } else {
            for (int x = 0; x < cols.size(); x++) {
                if ((x + 1) == cols.size()) {
                    ans += cols.get(x).printName() + " " + cols.get(x).getType();
                } else {
                    ans += cols.get(x).printName() + " " + cols.get(x).getType() + ",";
                }
            }
            ans += "\n";
            for (int y = 0; y < cols.get(0).size(); y++) {
                for (int x = 0; x < cols.size(); x++) {
                    if ((x + 1) == cols.size()) {
                        if (this.noValues.isNoValue(x, y)) {
                            ans += "NOVALUE";
                        }
                        else {
                            ans += getElement(x, y);
                        }
                    } else {
                        if (this.noValues.isNoValue(x, y)) {
                            ans += "NOVALUE" + ",";
                        }
                        else {
                            ans += getElement(x, y) + ",";
                        }
                    }
                }
                ans += "\n";
            }
        }
        return ans;
    }
}