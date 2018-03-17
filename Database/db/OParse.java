package db;

import java.util.regex.Pattern;
import java.util.regex.Matcher;

import java.util.StringJoiner;

public class OParse {
    // Various common constructs, simplifies parsing.
    private static final String REST = "\\s*(.*)\\s*",
            COMMA = "\\s*,\\s*",
            AND = "\\s+and\\s+";

    // Stage 1 syntax, contains the command name.
    private static final Pattern CREATE_CMD = Pattern.compile("create table " + REST),
            LOAD_CMD = Pattern.compile("load " + REST),
            STORE_CMD = Pattern.compile("store " + REST),
            DROP_CMD = Pattern.compile("drop table " + REST),
            INSERT_CMD = Pattern.compile("insert into " + REST),
            PRINT_CMD = Pattern.compile("print " + REST),
            SELECT_CMD = Pattern.compile("select " + REST);

    // Stage 2 syntax, contains the clauses of commands.
    private static final Pattern CREATE_NEW = Pattern.compile("(\\S+)\\s+\\(\\s*(\\S+\\s+\\S+\\s*" +
            "(?:,\\s*\\S+\\s+\\S+\\s*)*)\\)"),
            SELECT_CLS = Pattern.compile("([^,]+?(?:,[^,]+?)*)\\s+from\\s+" +
                    "(\\S+\\s*(?:,\\s*\\S+\\s*)*)(?:\\s+where\\s+" +
                    "([\\w\\s+\\-*/'<>=!.]+?(?:\\s+and\\s+" +
                    "[\\w\\s+\\-*/'<>=!.]+?)*))?"),
            CREATE_SEL = Pattern.compile("(\\S+)\\s+as select\\s+" +
                    SELECT_CLS.pattern()),
            INSERT_CLS = Pattern.compile("(\\S+)\\s+values\\s+(.+?" +
                    "\\s*(?:,\\s*.+?\\s*)*)");

    public static String eval(String query, Database myDatabase) {
        Matcher m;
        if ((m = CREATE_CMD.matcher(query)).matches()) {
            return createTable(m.group(1), myDatabase);
        } else if ((m = LOAD_CMD.matcher(query)).matches()) {
            return myDatabase.load(m.group(1));
        } else if ((m = STORE_CMD.matcher(query)).matches()) {
            return myDatabase.store(m.group(1));
        } else if ((m = DROP_CMD.matcher(query)).matches()) {
            return myDatabase.drop(m.group(1));
        } else if ((m = INSERT_CMD.matcher(query)).matches()) {
            return insertRow(m.group(1), myDatabase);
        } else if ((m = PRINT_CMD.matcher(query)).matches()) {
            return myDatabase.printTable(m.group(1));
        } else if ((m = SELECT_CMD.matcher(query)).matches()) {
            return myDatabase.select(m.group(1));
        } else {
            return "ERROR: Malformed query.";
        }
    }
    public static void main(String[] args) {
        Database lol = new Database();
        lol.transact("create table T1 (x int, y int)");
    }
    private static String createTable(String expr, Database myDatabase) {
        Matcher m;
        if ((m = CREATE_NEW.matcher(expr)).matches()) {
            String[] colsToBeMade = m.group(2).split("\\s*,\\s*|\\s+");
            return myDatabase.createNewTable(m.group(1), colsToBeMade);
        } else if ((m = CREATE_SEL.matcher(expr)).matches()) {
            return myDatabase.createSelectedTable(m.group(1), m.group(2), m.group(3), m.group(4));
        } else {
            return "ERROR: Malformed create expression.";
        }
    }

    private static void createNewTable(String name, String[] cols) {
        StringJoiner joiner = new StringJoiner(", ");
        for (int i = 0; i < cols.length-1; i++) {
            joiner.add(cols[i]);
        }

        String colSentence = joiner.toString() + " and " + cols[cols.length-1];
        System.out.printf("You are trying to create a table named %s with the columns %s\n", name, colSentence);
    }

    private static void createSelectedTable(String name, String exprs, String tables, String conds) {
        System.out.printf("You are trying to create a table named %s by selecting these expressions:" +
                " '%s' from the join of these tables: '%s', filtered by these conditions: '%s'\n", name, exprs, tables, conds);
    }

    private static String insertRow(String expr, Database myDatabase) {
        Matcher m = INSERT_CLS.matcher(expr);
        if (!m.matches()) {
            return "ERROR: Malformed insert.";
        }
        String[] values = m.group(2).split("\\s*,\\s*");
        return myDatabase.insertRow(m.group(1), values);

    }

    private static void select(String expr) {
        Matcher m = SELECT_CLS.matcher(expr);
        if (!m.matches()) {
            System.err.printf("Malformed select: %s\n", expr);
            return;
        }

        select(m.group(1), m.group(2), m.group(3));
    }

    private static void select(String exprs, String tables, String conds) {
        System.out.printf("You are trying to select these expressions:" +
                " '%s' from the join of these tables: '%s', filtered by these conditions: '%s'\n", exprs, tables, conds);
    }
}
