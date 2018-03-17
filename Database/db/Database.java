package db;

import java.util.ArrayList;
import java.io.File;
import java.util.Scanner;
import java.io.IOException;
import java.io.FileWriter;
import java.io.BufferedWriter;
import java.util.TreeSet;

public class Database {
    private ArrayList<Table> tables;

    public Database() {
        tables = new ArrayList<>();
    }

    public boolean checkIfTableExists(String T) {
        for (int x = 0; x < tables.size(); x++) {
            if (tables.get(x).printName().equals(T)) {
                return true;
            }
        }
        return false;
    }

    public void removeTable(String name) {
        for (int x = 0; x < tables.size(); x++) {
            if (tables.get(x).printName().equals(name)) {
                tables.remove(x);
            }
        }
    }

    public Table findTable(String name) {
        if (!checkIfTableExists(name)) {
            throw new NullPointerException("ERROR: This table does not exist in this database.");
        }
        for (int x = 0; x < tables.size(); x++) {
            if (tables.get(x).printName().equals(name)) {
                return tables.get(x);
            }
        }
        return null;
    }

    public String createNewTable(String name, String[] cols) {
        Table newTable = new Table(name);
        for (int x = 0; x < cols.length; x += 2) {
            if (!cols[x + 1].equals("int") && !cols[x + 1].equals("float")
                    && !cols[x + 1].equals("string")) {
                return "ERROR: Invalid column type.";
            }
            Column c = new Column(cols[x], cols[x + 1]);
            newTable.insertCol(c);
        }
        this.insertTable(newTable);
        return "";
    }

    public String createSelectedTable(String name, String cols, String tabs, String conds) {
        Table from = from(tabs);
        Table where = where(conds, from);
        Table selected = selectTablev2(cols, where);
        selected.tableName = name;
        this.insertTable(selected);
        return "";
    }

    public String drop(String name) {
        if (!checkIfTableExists(name)) {
            return "ERROR: Table does not exist.";
        }
        for (int x = 0; x < tables.size(); x++) {
            if (tables.get(x).printName().equals(name)) {
                tables.remove(x);
            }
        }
        return "";
    }

    private ArrayList<String> getFile(String fileName) {

        StringBuilder result = new StringBuilder("");
        File file = new File(fileName);

        try (Scanner scanner = new Scanner(file)) {
            ArrayList<String> colsAndValues = new ArrayList<>();
            while (scanner.hasNextLine()) {
                String line = scanner.nextLine();
                String stringedLined = line.toString();
                colsAndValues.add(stringedLined);
            }

            scanner.close();
            return colsAndValues;
        } catch (IOException e) {
            return null;
        }
    }

    public String load(String name) {
        Table table = new Table(name);
        File file = new File(name + ".tbl");
        if (!file.exists()) {
            return "ERROR: File does not exist.";
        }
        ArrayList<String> newTab = this.getFile(name + ".tbl");
        for (int x = 0; x < newTab.size(); x++) {
            if (x == 0) {
                String cols = newTab.get(0);
                String[] colsToBeMade = cols.split("\\s*,\\s*|\\s+");
                for (int i = 0; i < colsToBeMade.length; i += 2) {
                    Column c = new Column(colsToBeMade[i], colsToBeMade[i + 1]);
                    table.insertCol(c);
                }
                for (int i = 0; i < table.getColLen(); i++) {
                    if (!table.getCol().get(i).getType().equals("int")
                            && !table.getCol().get(i).getType().equals("float")
                            && !table.getCol().get(i).getType().equals("string")) {
                        return "ERROR: Malformed table is not valid.";
                    }
                }
            } else {
                String values = newTab.get(x);
                String[] valuesToBeAdded = values.split("\\s*,\\s*");
                if (valuesToBeAdded.length != table.getColLen()) {
                    return "ERROR: Number of Values != number of columns.";
                }
                for (int y = 0; y < table.getColLen(); y++) {
                    if (table.cols.get(y).printType().equals("Integer")) {
                        if (valuesToBeAdded[y].equals("NOVALUE")) {
                            table.cols.get(y).add(0);
                            int xIndex = table.columnIndex(table.getCol().get(y));
                            int yIndex = x - 1;
                            table.noValues.put(xIndex, yIndex);
                        } else {
                            table.cols.get(y).add(Integer.parseInt(valuesToBeAdded[y]));
                        }
                    } else if (table.cols.get(y).printType().equals("Float")) {
                        if (valuesToBeAdded[y].equals("NOVALUE")) {
                            table.cols.get(y).add(0.0F);
                            int xIndex = table.columnIndex(table.getCol().get(y));
                            int yIndex = x - 1;
                            table.noValues.put(xIndex, yIndex);
                        } else if (!valuesToBeAdded[y].contains(".")) {
                            return "ERROR: Should not parse int to float.";
                        } else {
                            table.cols.get(y).add(Float.parseFloat(valuesToBeAdded[y]));
                        }
                    } else if (table.cols.get(y).printType().equals("String")) {
                        if (valuesToBeAdded[y].equals("NOVALUE")) {
                            table.cols.get(y).add("''");
                            int xIndex = table.columnIndex(table.getCol().get(y));
                            int yIndex = x - 1;
                            table.noValues.put(xIndex, yIndex);
                        } else {
                            if (Database.isString(valuesToBeAdded[y])) {
                                table.cols.get(y).add(valuesToBeAdded[y]);
                            } else {
                                return "ERROR: Cannot add ints or floats to string column.";
                            }
                        }
                    } else {
                        return "ERROR: Invalid type insertion.";
                    }
                }
            }
        }
        if (malFormed(table)) {
            return "ERROR: Malformed table.";
        }
        if (this.checkIfTableExists(name)) {
            this.removeTable(name);
        }
        this.tables.add(table);
        return "";
    }

    public boolean malFormed(Table t) {
        if (t.getCol().size() == 0) {
            return true;
        }
        int length = t.getCol().get(0).size();
        for (int x = 0; x < t.getCol().size(); x++) {
            if (t.getCol().get(x).size() != length) {
                return true;
            }
        }
        return false;
    }

    public String store(String name) {
        File file = new File(name + ".tbl");
        if (!this.checkIfTableExists(name)) {
            return "ERROR: Table does not exist in database.";
        }
        if (file.exists()) {
            try {
                FileWriter fstream = new FileWriter(file, false);
                BufferedWriter out = new BufferedWriter(fstream);
                out.write(findTable(name).printContents());
                out.close();
                return "";
            } catch (IOException i) {
                return "ERROR: Overwrite exception.";
            }
        } else {
            try {
                file.createNewFile();
                FileWriter fstream = new FileWriter(findTable(name).printName() + ".tbl");
                BufferedWriter out = new BufferedWriter(fstream);
                out.write(findTable(name).printContents());
                out.close();
                return "";
            } catch (IOException i) {
                return "ERROR: File creation exception.";
            }
        }
    }


    public String select(String whatToSelect) {
        String[] colsWithOps = whatToSelect.split("\\s*from\\s*");
        String[] listOfTablesAndConditions = colsWithOps[1].split("\\s*where\\s*");
        Table ret = from(listOfTablesAndConditions[0]);
        if (listOfTablesAndConditions.length == 2) {
            ret = where(listOfTablesAndConditions[1], ret);
        }
        ret = selectTablev2(colsWithOps[0], ret);
        return ret.printContents();
    }

    public static String getOperator(String s) {
        for (int x = 0; x < s.length(); x++) {
            if (Database.isOperator(s.substring(x, x + 1))) {
                return s.substring(x, x + 1);
            }
        }
        return "This is an individual column";
    }

    public Table selectTablev2(String columnOps, Table t) {
        if (columnOps.trim().substring(0, 1).equals("*")) {
            return t;
        }
        Table ret = new Table("anon");
        String[] chunksToBeEvaled = columnOps.split("\\s*,\\s*");
        for (String chunk : chunksToBeEvaled) {
            String trimmedChunk = chunk.trim();
            if (trimmedChunk.matches(".*\\bas\\b.*")) {
                String[] splitInHalf = trimmedChunk.split("\\s*as\\s*");
                String[] checkForOperations = splitInHalf[0].split("\\s*\\*|\\+|-|/\\s*");
                String[] trimmedOperations = new String[checkForOperations.length];
                int index = 0;
                for (String check : checkForOperations) {
                    trimmedOperations[index] = check.trim();
                    index++;
                }
                String operator = Database.getOperator(splitInHalf[0]);
                if (checkForOperations.length == 1) {
                    TripletOperation trip = new TripletOperation(trimmedOperations[0],
                            "This is an individual column", null);
                    Column toAdd = trip.decipher(t);
                    toAdd.colName = splitInHalf[1].trim();
                    ret.insertCol(toAdd);
                } else {
                    TripletOperation trip = new TripletOperation(trimmedOperations[0],
                            operator, trimmedOperations[1]);
                    Column toAdd = trip.decipher(t);
                    toAdd.colName = splitInHalf[1].trim();
                    ret.insertCol(toAdd);
                }
            } else {
                TripletOperation trip = new TripletOperation(trimmedChunk,
                        "This is an individual column", null);
                Column toAdd = trip.decipher(t);
                toAdd.colName = trimmedChunk;
                ret.insertCol(toAdd);
            }

        }
        return ret;
    }

    public static boolean isOperator(String str) {
        return (str.equals("+") || str.equals("-") || str.equals("*") || str.equals("/"));
    }

    public String insertRow(String name, String[] values) {
        if (!checkIfTableExists(name)) {
            return "ERROR: Table does not exist.";
        }
        Table table = findTable(name);
        if (table.getColLen() != values.length) {
            return "ERROR: Number of values != number of columns.";
        }
        try {
            for (int x = 0; x < values.length; x++) {
                if (table.getCol().get(x).printType().equals("Integer")) {
                    if (values[x].equals("NOVALUE")) {
                        table.getCol().get(x).add(0);
                        int xIndex = table.columnIndex(table.getCol().get(x));
                        int yIndex = table.getCol().get(x).size() - 1;
                        table.noValues.put(xIndex, yIndex);
                    } else {
                        table.getCol().get(x).add(Integer.parseInt(values[x]));
                    }
                } else if (table.getCol().get(x).printType().equals("Float")) {
                    if (values[x].equals("NOVALUE")) {
                        table.getCol().get(x).add(0.0F);
                        int xIndex = table.columnIndex(table.getCol().get(x));
                        int yIndex = table.getCol().get(x).size() - 1;
                        table.noValues.put(xIndex, yIndex);
                    } else {
                        try {
                            Integer.parseInt(values[x]);
                            return "ERROR: Should not parse float to int.";
                        } catch (NumberFormatException n) {
                            table.getCol().get(x).add(Float.parseFloat(values[x]));
                        }
                    }
                } else if (table.getCol().get(x).printType().equals("String")) {
                    if (values[x].equals("NOVALUE")) {
                        table.getCol().get(x).add("''");
                        int xIndex = table.columnIndex(table.getCol().get(x));
                        int yIndex = table.getCol().get(x).size() - 1;
                        table.noValues.put(xIndex, yIndex);
                    } else if (Database.isString(values[x])) {
                        table.getCol().get(x).add(values[x]);
                    } else {
                        return "ERROR: Cannot add string without quotes.";
                    }

                } else {
                    return "ERROR: Invalid type insertion.";
                }
            }
        } catch (NumberFormatException n) {
            return "ERROR: Tried converting value to wrong type.";
        }
        return "";
    }


    public String printTable(String name) {
        boolean exists = this.checkIfTableExists(name);
        if (exists) {
            return this.findTable(name).printContents();
        } else {
            return "ERROR: Table does not exist in database.";
        }
    }

    public Table from(String listOfTables) {
        String[] separatedTables = listOfTables.split("\\s*,\\s*|\\s+");
        ArrayList<Table> tabs = new ArrayList<>();
        for (String s : separatedTables) {
            tabs.add(this.findTable(s));
        }
        if (tabs.size() == 1) {
            return tabs.get(0);
        }
        Table result = Table.joins(tabs.get(0), tabs.get(1));
        for (int a = 2; a < tabs.size(); a++) {
            result = Table.joins(result, tabs.get(a));
        }
        return result;
    }

    public Table where(String conditions, Table source) {
        if (conditions == null) {
            return source;
        }
        ArrayList<Integer> success = new ArrayList<>();
        String[] separatedConditions = conditions.split("\\s*and\\s*|\\s+");
        ArrayList<Column> firstColumns = new ArrayList<>();
        for (int a = 0; a < separatedConditions.length; ) {
            firstColumns.add(Table.getColFromString(separatedConditions[a], source));
            a += 3;
        }
        ArrayList<String> conditionals = new ArrayList<>();
        for (int a = 1; a < separatedConditions.length; ) {
            conditionals.add(separatedConditions[a]);
            a += 3;
        }
        ArrayList<String> literalOrCol = new ArrayList<>();
        for (int a = 2; a < separatedConditions.length; ) {
            literalOrCol.add(separatedConditions[a]);
            a += 3;
        }
        int index = 0;
        for (String lOC : literalOrCol) {
            try {
                Float floatLiteral = Float.parseFloat(lOC);
                ArrayList<Integer> toAdd = callComparisonFlt(firstColumns.get(index),
                        conditionals.get(index), floatLiteral);
                success.addAll(toAdd);
            } catch (NumberFormatException nfe) {
                if (isString(lOC)) {
                    String stringLiteral = lOC;
                    ArrayList<Integer> toAdd = callComparisonStr(firstColumns.get(index),
                            conditionals.get(index), stringLiteral);
                    success.addAll(toAdd);
                } else {
                    Column colComparison = Table.getColFromString(lOC, source);
                    ArrayList<Integer> toAdd = callComparisonCol(firstColumns.get(index),
                            conditionals.get(index), colComparison);
                    success.addAll(toAdd);
                }
            }
            index++;
        }
        int numberOfComparisons = conditionals.size();
        ArrayList<Integer> rowsToKeep = enough(success, numberOfComparisons);
        Table ret = new Table("anonymous");
        for (Column c : source.cols) {
            Column toAdd = c.obtainPartsOfColumnEntriesForJoins(rowsToKeep);
            ret.insertCol(toAdd);
        }
        return ret;
    }

    public static boolean isString(String str) {
        for (int posit = 0; posit < str.length(); posit++) {
            if (str.substring(posit, posit + 1).equals("'")) {
                return true;
            }
        }
        return false;
    }

    //Checks if an index occurs the required amount of times
    public static ArrayList<Integer> enough(ArrayList<Integer> aL, int requirement) {
        ArrayList<Integer> unique = new ArrayList<>(new TreeSet<Integer>(aL));
        ArrayList<Integer> goodEnough = new ArrayList<>();
        for (Integer a : unique) {
            if (occurrences(aL, a) == requirement) {
                goodEnough.add(a);
            }
        }
        return goodEnough;
    }

    public static int occurrences(ArrayList<Integer> aL, int occ) {
        int count = 0;
        for (Integer a : aL) {
            if (a.equals(occ)) {
                count++;
            }
        }
        return count;
    }

    public static ArrayList<Integer> callComparisonFlt(Column col, String operator, Float flt) {
        if (operator.equals(">")) {
            return col.GreaterUnary(flt);
        } else if (operator.equals("<")) {
            return col.LesserUnary(flt);
        } else if (operator.equals(">=")) {
            return col.GTEUnary(flt);
        } else if (operator.equals("<=")) {
            return col.LTEUnary(flt);
        } else if (operator.equals("==")) {
            return col.EqualUnary(flt);
        } else {
            return col.NotEqualUnary(flt);
        }
    }

    public static ArrayList<Integer> callComparisonStr(Column col, String operator, String str) {
        if (operator.equals(">")) {
            return col.GreaterUnary(str);
        } else if (operator.equals("<")) {
            return col.LesserUnary(str);
        } else if (operator.equals(">=")) {
            return col.GTEUnary(str);
        } else if (operator.equals("<=")) {
            return col.LTEUnary(str);
        } else if (operator.equals("==")) {
            return col.EqualUnary(str);
        } else {
            return col.NotEqualUnary(str);
        }
    }

    public static ArrayList<Integer> callComparisonCol(Column col, String operator, Column col2) {
        if (operator.equals(">")) {
            return col.GreaterBinary(col2);
        } else if (operator.equals("<")) {
            return col.LesserBinary(col2);
        } else if (operator.equals(">=")) {
            return col.GTEBinary(col2);
        } else if (operator.equals("<=")) {
            return col.LTEBinary(col2);
        } else if (operator.equals("==")) {
            return col.EqualBinary(col2);
        } else {
            return col.NotEqualBinary(col2);
        }
    }

    public void insertTable(Table... t) {
        for (Table tab : t) {
            tables.add(tab);
        }
    }

    public String transact(String query) {
        try {
            return OParse.eval(query, this);
        } catch (ArrayIndexOutOfBoundsException a) {
            return "ERROR: IndexOutOfBoundsException.";
        } catch (IllegalArgumentException i) {
            return "ERROR: IllegalArgumentException.";
        } catch (NullPointerException n) {
            return "ERROR: NullPointerException.";
        } catch (ClassCastException c) {
            return "ERROR: ClassCastException.";
        }
    }
}
